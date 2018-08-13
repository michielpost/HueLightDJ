using HueLightDJ.Effects;
using HueLightDJ.Effects.Base;
using HueLightDJ.Web.Hubs;
using HueLightDJ.Web.Models;
using Microsoft.AspNetCore.SignalR;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
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
    private static List<TypeInfo> TouchEffectTypes { get; set; }
    private static Dictionary<EntertainmentLayer, RunningEffectInfo> layerInfo = new Dictionary<EntertainmentLayer, RunningEffectInfo>();
    private static CancellationTokenSource autoModeCts;

    public static List<TypeInfo> GetEffectTypes()
    {
      if (EffectTypes == null)
      {
        var all = LoadAllEffects<IHueEffect>();
        EffectTypes = all;
      }

      return EffectTypes;
    }

    public static List<TypeInfo> GetGroupEffectTypes()
    {
      if (GroupEffectTypes == null)
      {
        var all = LoadAllEffects<IHueGroupEffect>();
        GroupEffectTypes = all;
      }

      return GroupEffectTypes;
    }

    public static List<TypeInfo> GetTouchEffectTypes()
    {
      if (TouchEffectTypes == null)
      {
        var all = LoadAllEffects<IHueTouchEffect>();
        TouchEffectTypes = all;
      }

      return TouchEffectTypes;
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

        if(!string.IsNullOrEmpty(hueEffectAtt.DefaultColor))
        {
          effect.Color = hueEffectAtt.DefaultColor;
          effect.IsRandom = false;
        }

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

        if (!string.IsNullOrEmpty(hueEffectAtt.DefaultColor))
        {
          effect.Color = hueEffectAtt.DefaultColor;
          effect.IsRandom = false;
        }

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
      autoModeCts?.Cancel();
      autoModeCts = new CancellationTokenSource();

      Task.Run(async () =>
      {
        while(!autoModeCts.IsCancellationRequested)
        {
          StartRandomEffect();
          await Task.Delay(TimeSpan.FromSeconds(6));
        }
      }, autoModeCts.Token);
    }

    public static void StopAutoMode()
    {
      autoModeCts?.Cancel();
    }

    public static bool IsAutoModeRunning()
    {
      if (autoModeCts == null || autoModeCts.IsCancellationRequested)
        return false;

      return true;
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

        var isBaseLayer = hueEffectAtt.IsBaseEffect && iteratorMode != IteratorEffectMode.Single && iteratorMode != IteratorEffectMode.RandomOrdered;
        var layer = GetLayer(isBaseLayer);

        if (layerInfo.ContainsKey(layer))
        {
          //Cancel currently running job
          layerInfo[layer].CancellationTokenSource?.Cancel();
        }

        CancellationTokenSource cts = new CancellationTokenSource();
        layerInfo[layer] = new RunningEffectInfo() { Name = hueEffectAtt.Name, CancellationTokenSource = cts };

        Func<TimeSpan> waitTime = () => StreamingSetup.WaitTime;
        RGBColor? color = null;
        if (!string.IsNullOrEmpty(colorHex))
          color = new RGBColor(colorHex);

       
        if(isGroupEffect)
        {
          //get group
          var selectedGroup = GroupService.GetAll(layer).Where(x => x.Name == group).Select(x => x.Lights).FirstOrDefault();

          StartEffect(cts.Token, selectedEffect, selectedGroup, group, waitTime, color, iteratorMode, secondaryIteratorMode);
        }
        else
        {
          StartEffect(cts.Token, selectedEffect, layer, waitTime, color);
        }
        
      }
    }

    private static void StartEffect(CancellationToken ctsToken, TypeInfo selectedEffect, IEnumerable<IEnumerable<EntertainmentLight>> group, string groupName, Func<TimeSpan> waitTime, RGBColor? color, IteratorEffectMode iteratorMode = IteratorEffectMode.All, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All)
    {
      MethodInfo methodInfo = selectedEffect.GetMethod("Start");

      //get group
      if (group == null)
        group = GroupService.GetRandomGroup();

      object[] parametersArray = new object[] { group, waitTime, color, iteratorMode, secondaryIteratorMode, ctsToken};

     
      object classInstance = Activator.CreateInstance(selectedEffect, null);
      methodInfo.Invoke(classInstance, parametersArray);

      var hub = (IHubContext<StatusHub>)Startup.ServiceProvider.GetService(typeof(IHubContext<StatusHub>));
      hub.Clients.All.SendAsync("StartingEffect", $"Starting: {selectedEffect.Name} {groupName}, {iteratorMode}-{secondaryIteratorMode} {color?.ToHex()}",
          new EffectLogMsg()
          {
            EffectType = "group",
            Name = selectedEffect.Name,
            RGBColor = color?.ToHex(),
            Group = groupName,
            IteratorMode = iteratorMode.ToString(),
            SecondaryIteratorMode = secondaryIteratorMode.ToString(),

          });

    }

    private static void StartTouchEffect(CancellationToken ctsToken, TypeInfo selectedEffect, Func<TimeSpan> waitTime, RGBColor? color, double x, double y)
    {
      MethodInfo methodInfo = selectedEffect.GetMethod("Start");

      var layer = GetLayer(isBaseLayer: false);

      object[] parametersArray = new object[] { layer, waitTime, color, ctsToken, x, y };

      object classInstance = Activator.CreateInstance(selectedEffect, null);
      methodInfo.Invoke(classInstance, parametersArray);
    }

    private static void StartEffect(CancellationToken ctsToken, TypeInfo selectedEffect, EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color)
    {
      MethodInfo methodInfo = selectedEffect.GetMethod("Start");
      object[] parametersArray = new object[] { layer, waitTime, color, ctsToken };

      object classInstance = Activator.CreateInstance(selectedEffect, null);
      methodInfo.Invoke(classInstance, parametersArray);

      var hub = (IHubContext<StatusHub>)Startup.ServiceProvider.GetService(typeof(IHubContext<StatusHub>));
      hub.Clients.All.SendAsync("StartingEffect", $"Starting: {selectedEffect.Name} {color?.ToHex()}", new EffectLogMsg() { Name = selectedEffect.Name, RGBColor = color?.ToHex() });

    }

    public static void StartRandomEffect()
    {
      var r = new Random();

      var all = GetEffectTypes();
      var allGroup = GetGroupEffectTypes();

      if (r.NextDouble() <= 0.7)
        StartRandomGroupEffect();
      else
      {
        string effect = all
          .Where(x => x.Name != typeof(ChristmasEffect).Name)
          .Where(x => x.Name != typeof(AllOffEffect).Name)
          .Where(x => x.Name != typeof(SetColorEffect).Name)
          .OrderBy(x => Guid.NewGuid()).FirstOrDefault().Name;

        GenerateRandomEffectSettings(out RGBColor hexColor, out _, out _);

        StartEffect(effect, hexColor.ToHex());
      }

    }

    private static void StartRandomGroupEffect(bool useMultipleEffects = true)
    {
      Func<TimeSpan> waitTime = () => StreamingSetup.WaitTime;

      Random r = new Random();
      var allGroupEffects = GetGroupEffectTypes();

      //Always run on baselayer
      var layer = GetLayer(isBaseLayer: true);

      //Random group that supports multiple effects
      var group = GroupService.GetAll(layer).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
      var lightList = group.Lights.ToList();

      int minEffects = Math.Min(lightList.Count, group.MaxEffects);

      //Get same number of effects as groups in the light list
      var effects = allGroupEffects.OrderBy(x => Guid.NewGuid()).Take(minEffects).ToList();

      //Chunk the group by the number of effects we have
      var chunks = group.Lights.ChunkByGroupNumber(effects.Count).ToList();

      //Cancel current
      if (layerInfo.ContainsKey(layer))
      {
        //Cancel currently running job
        layerInfo[layer].CancellationTokenSource?.Cancel();
      }

      CancellationTokenSource cts = new CancellationTokenSource();
      layerInfo[layer] = new RunningEffectInfo() { Name = "Double random", CancellationTokenSource = cts };


      for (int i = 0; i < chunks.Count; i++)
      {
        var section = chunks[i];
        GenerateRandomEffectSettings(out RGBColor hexColor, out IteratorEffectMode iteratorMode, out IteratorEffectMode iteratorSecondaryMode);

        StartEffect(cts.Token, effects[i], section, group.Name, waitTime, hexColor, iteratorMode, iteratorSecondaryMode);
      }


    }

    private static void GenerateRandomEffectSettings(out RGBColor hexColor, out IteratorEffectMode iteratorMode, out IteratorEffectMode iteratorSecondaryMode)
    {
      Random r = new Random();
      hexColor = RGBColor.Random(r);
      while (hexColor.R < 0.15 && hexColor.G < 0.15 && hexColor.B < 0.15)
        hexColor = RGBColor.Random(r);

      Array values = Enum.GetValues(typeof(IteratorEffectMode));
      iteratorMode = (IteratorEffectMode)values.GetValue(r.Next(values.Length));
      iteratorSecondaryMode = (IteratorEffectMode)values.GetValue(r.Next(values.Length));

      //Bounce and Single are no fun for random mode
      if (iteratorMode == IteratorEffectMode.Bounce || iteratorMode == IteratorEffectMode.Single)
        iteratorMode = IteratorEffectMode.Cycle;
      else if (iteratorMode == IteratorEffectMode.RandomOrdered) //RandomOrdered only runs once
        iteratorMode = IteratorEffectMode.Random;
    }

    private static EntertainmentLayer GetLayer(bool isBaseLayer)
    {
      if (isBaseLayer)
        return StreamingSetup.Layers.First();

      return StreamingSetup.Layers.Last();
    }

    private static List<TypeInfo> LoadAllEffects<T>()
    {
      Dictionary<TypeInfo, HueEffectAttribute> result = new Dictionary<TypeInfo, HueEffectAttribute>();

      //Get all effects that implement IHueEffect
      Assembly ass = typeof(T).Assembly;

      foreach (TypeInfo ti in ass.DefinedTypes)
      {
        if (ti.ImplementedInterfaces.Contains(typeof(T)))
        {
          var hueEffectAtt = ti.GetCustomAttribute<HueEffectAttribute>();

          if (hueEffectAtt != null)
          {
            result.Add(ti, hueEffectAtt);
          }
        }
      }

      return result.OrderBy(x => x.Value.Order).Select(x => x.Key).ToList();
    }

    public static void CancelAllEffects()
    {
      StopAutoMode();

      foreach(var layer in layerInfo)
      {
        layer.Value?.CancellationTokenSource?.Cancel();
      }
    }

    public static void StartRandomTouchEffect(double x, double y)
    {
      var effectLayer = GetLayer(isBaseLayer: false);

      var randomTouch = GetTouchEffectTypes().OrderBy(_ => Guid.NewGuid()).FirstOrDefault();

      Func<TimeSpan> waitTime = () => StreamingSetup.WaitTime;

      StartTouchEffect(CancellationToken.None, randomTouch, waitTime, null, x, y);
    }
  }
}
