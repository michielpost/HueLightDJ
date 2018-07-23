using HueLightDJ.Effects;
using HueLightDJ.Effects.Base;
using HueLightDJ.Web.Hubs;
using HueLightDJ.Web.Models;
using Microsoft.AspNetCore.SignalR;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Streaming
{
  public static class EffectService
  {
    private static List<TypeInfo> EffectTypes { get; set; }
    private static List<TypeInfo> GroupEffectTypes { get; set; }
    private static Dictionary<EntertainmentLayer, RunningEffectInfo> layerInfo = new Dictionary<EntertainmentLayer, RunningEffectInfo>();
    private static CancellationTokenSource cts;

    public static List<TypeInfo> GetEffectTypes()
    {
      if (EffectTypes == null)
      {
        var all = LoadAllEffects();
        EffectTypes = all;
      }

      return EffectTypes;
    }

    public static List<TypeInfo> GetGroupEffectTypes()
    {
      if (GroupEffectTypes == null)
      {
        var all = LoadAllGroupEffects();
        GroupEffectTypes = all;
      }

      return GroupEffectTypes;
    }

    public static EffectsVM GetEffectViewModels()
    {
      var all = GetEffectTypes();
      var groupEffectsTypes = GetGroupEffectTypes();
      var groups = GroupService.GetAll();

      List<EffectViewModel> baseEffects = new List<EffectViewModel>();
      List<EffectViewModel> shortEffects = new List<EffectViewModel>();
      List<EffectViewModel> groupEffects = new List<EffectViewModel>();
      foreach (var type in all)
      {
        var hueEffectAtt = type.GetCustomAttribute<HueEffectAttribute>();

        var effect = new EffectViewModel();
        effect.Name = hueEffectAtt.Name;
        effect.TypeName = type.Name;
        effect.HasColorPicker = hueEffectAtt.HasColorPicker;

        if (hueEffectAtt.IsBaseEffect)
          baseEffects.Add(effect);
        else
          shortEffects.Add(effect);
      }

      foreach (var type in groupEffectsTypes)
      {
        var hueEffectAtt = type.GetCustomAttribute<HueEffectAttribute>();

        var effect = new EffectViewModel();
        effect.Name = hueEffectAtt.Name;
        effect.TypeName = type.Name;
        effect.HasColorPicker = hueEffectAtt.HasColorPicker;

        groupEffects.Add(effect);
      }

      List<string> iteratorNames = new List<string>();
      foreach(var name in Enum.GetNames(typeof(IteratorEffectMode)))
      {
        iteratorNames.Add(name);
      }

      List<string> secondaryIteratorNames = new List<string>() {
        IteratorEffectMode.All.ToString(),
        IteratorEffectMode.AllIndividual.ToString(),
        IteratorEffectMode.Bounce.ToString(),
        IteratorEffectMode.Single.ToString(),
        IteratorEffectMode.Random.ToString(),
      };

      var vm = new EffectsVM();
      vm.BaseEffects = baseEffects;
      vm.ShortEffects = shortEffects;
      vm.GroupEffects = groupEffects;
      vm.Groups = groups.Select(x => new GroupInfoViewModel() { Name = x.Name }).ToList();
      vm.IteratorModes = iteratorNames;
      vm.SecondaryIteratorModes = secondaryIteratorNames;

      return vm;
    }

    public static void StartAutoMode()
    {
      cts?.Cancel();
      cts = new CancellationTokenSource();

      Task.Run(async () =>
      {
        while(!cts.IsCancellationRequested)
        {
          StartRandomEffect();
          await Task.Delay(TimeSpan.FromSeconds(6));
        }
      }, cts.Token);
    }

    public static void StopAutoMode()
    {
      cts?.Cancel();
    }

    public static void StartEffect(string typeName, string colorHex, string group = null, IteratorEffectMode iteratorMode = IteratorEffectMode.All, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All)
    {

      var hub = (IHubContext<StatusHub>)Startup.ServiceProvider.GetService(typeof(IHubContext<StatusHub>));

      var all = GetEffectTypes();
      var allGroup = GetGroupEffectTypes();

      var effectType = all.Where(x => x.Name == typeName).FirstOrDefault();
      var groupEffectType = allGroup.Where(x => x.Name == typeName).FirstOrDefault();

      bool isGroupEffect = groupEffectType != null && !string.IsNullOrEmpty(group);
      var selectedEffect = isGroupEffect ? groupEffectType : effectType;

      if (selectedEffect != null)
      {
        var hueEffectAtt = selectedEffect.GetCustomAttribute<HueEffectAttribute>();

        var isBaseLayer = hueEffectAtt.IsBaseEffect && iteratorMode != IteratorEffectMode.Single;
        var layer = GetLayer(isBaseLayer);

        if (layerInfo.ContainsKey(layer))
        {
          //Cancel currently running job
          layerInfo[layer].CancellationTokenSource?.Cancel();
        }

        CancellationTokenSource cts = new CancellationTokenSource();
        layerInfo[layer] = new RunningEffectInfo() { Name = hueEffectAtt.Name, CancellationTokenSource = cts };


        var waitTime = StreamingSetup.WaitTime;
        RGBColor? color = null;
        if (!string.IsNullOrEmpty(colorHex))
          color = new RGBColor(colorHex);

        MethodInfo methodInfo = selectedEffect.GetMethod("Start");

        object result = null;
        object[] parametersArray = new object[] { layer, waitTime, color, cts.Token };

        if(isGroupEffect && !string.IsNullOrEmpty(group))
        {
          //get group
          var selectedGroup = GroupService.GetAll().Where(x => x.Name == group).Select(x => x.Lights).FirstOrDefault();

          if (selectedGroup == null)
            selectedGroup = GroupService.GetRandomGroup();

          parametersArray = new object[] { selectedGroup, waitTime, color, iteratorMode, secondaryIteratorMode, cts.Token };

          hub.Clients.All.SendAsync("StatusMsg", $"{DateTime.Now} | Starting: {selectedEffect.Name} {group}, {iteratorMode}-{secondaryIteratorMode} {color?.ToHex()}");

        }
        else
        {
          hub.Clients.All.SendAsync("StatusMsg", $"{DateTime.Now} | Starting: {selectedEffect.Name} {color?.ToHex()}");
        }

        object classInstance = Activator.CreateInstance(selectedEffect, null);
        result = methodInfo.Invoke(classInstance, parametersArray);
      }
    }

    public static void StartRandomEffect()
    {
      var r = new Random();

      var all = GetEffectTypes();
      var allGroup = GetGroupEffectTypes();

      string effect;
      if(r.NextDouble() <= 0.7)
        effect = allGroup.OrderBy(x => Guid.NewGuid()).FirstOrDefault().Name;
      else
        effect = all.Where(x => x.Name != typeof(ChristmasEffect).Name).OrderBy(x => Guid.NewGuid()).FirstOrDefault().Name;

      var group = GroupService.GetAll().OrderBy(x => Guid.NewGuid()).FirstOrDefault().Name;

      var hexColor = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
      while(hexColor.R < 0.15 && hexColor.G < 0.15 && hexColor.B < 0.15)
        hexColor = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());

      Array values = Enum.GetValues(typeof(IteratorEffectMode));
      var iteratorMode = (IteratorEffectMode)values.GetValue(r.Next(values.Length));
      var iteratorSecondaryMode = (IteratorEffectMode)values.GetValue(r.Next(values.Length));

      //Bounce and Single are no fun for random mode
      if (iteratorMode == IteratorEffectMode.Bounce || iteratorMode == IteratorEffectMode.Single)
        iteratorMode = IteratorEffectMode.Cycle;

      StartEffect(effect, hexColor.ToHex(), group, iteratorMode, iteratorSecondaryMode);

    }

    private static EntertainmentLayer GetLayer(bool isBaseLayer)
    {
      if (isBaseLayer)
        return StreamingSetup.Layers.First();

      return StreamingSetup.Layers.Last();
    }

    private static List<TypeInfo> LoadAllEffects()
    {
      List<TypeInfo> result = new List<TypeInfo>();

      //Get all effects that implement IHueEffect
      Assembly ass = typeof(IHueEffect).Assembly;

      foreach (TypeInfo ti in ass.DefinedTypes)
      {
        if (ti.ImplementedInterfaces.Contains(typeof(IHueEffect)))
        {
          var hueEffectAtt = ti.GetCustomAttribute<HueEffectAttribute>();

          if (hueEffectAtt != null)
          {
            result.Add(ti);
          }
        }
      }

      return result;
    }

    private static List<TypeInfo> LoadAllGroupEffects()
    {
      List<TypeInfo> result = new List<TypeInfo>();

      //Get all effects that implement IHueGroupEffect
      Assembly ass = typeof(IHueGroupEffect).Assembly;

      foreach (TypeInfo ti in ass.DefinedTypes)
      {
        if (ti.ImplementedInterfaces.Contains(typeof(IHueGroupEffect)))
        {
          var hueEffectAtt = ti.GetCustomAttribute<HueEffectAttribute>();

          if (hueEffectAtt != null)
          {
            result.Add(ti);
          }
        }
      }

      return result;
    }

    public static void CancelAllEffects()
    {
      foreach(var layer in layerInfo)
      {
        layer.Value?.CancellationTokenSource?.Cancel();
      }
    }
  }
}
