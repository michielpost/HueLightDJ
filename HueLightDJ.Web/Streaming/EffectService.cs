using HueLightDJ.Effects;
using HueLightDJ.Effects.Base;
using HueLightDJ.Web.Models;
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

      var vm = new EffectsVM();
      vm.BaseEffects = baseEffects;
      vm.ShortEffects = shortEffects;
      vm.GroupEffects = groupEffects;
      vm.Groups = groups.Select(x => new GroupInfoViewModel() { Name = x.Name }).ToList();
      vm.IteratorModes = iteratorNames;
      return vm;
    }

    public static void StartEffect(string typeName, string colorHex, string group = null, IteratorEffectMode iteratorMode = IteratorEffectMode.All)
    {
      var all = GetEffectTypes();
      var allGroup = GetGroupEffectTypes();

      var effectType = all.Where(x => x.Name == typeName).FirstOrDefault();
      var groupEffectType = allGroup.Where(x => x.Name == typeName).FirstOrDefault();
      var selectedEffect = string.IsNullOrEmpty(group) ? effectType : groupEffectType;

      if (selectedEffect != null)
      {
        var hueEffectAtt = selectedEffect.GetCustomAttribute<HueEffectAttribute>();

        var layer = GetLayer(hueEffectAtt.IsBaseEffect);

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

        if(!string.IsNullOrEmpty(group))
        {
          IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All;

          //get group
          var selectedGroup = GroupService.GetAll().Where(x => x.Name == group).Select(x => x.Lights).FirstOrDefault();
          parametersArray = new object[] { selectedGroup, waitTime, color, iteratorMode, secondaryIteratorMode, cts.Token };
        }

        object classInstance = Activator.CreateInstance(selectedEffect, null);
        result = methodInfo.Invoke(classInstance, parametersArray);
      }
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
