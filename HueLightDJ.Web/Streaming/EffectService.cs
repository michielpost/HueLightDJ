using HueLightDJ.Effects;
using HueLightDJ.Effects.Base;
using HueLightDJ.Web.Models;
using Q42.HueApi.ColorConverters;
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

    public static List<EffectViewModel> GetEffectViewModels()
    {
      var all = GetEffectTypes();

      List<EffectViewModel> result = new List<EffectViewModel>();
      foreach (var type in all)
      {
        var hueEffectAtt = type.GetCustomAttribute<HueEffectAttribute>();

        var effect = new EffectViewModel();
        effect.Name = hueEffectAtt.Name;
        effect.TypeName = type.Name;
        effect.HasColorPicker = hueEffectAtt.HasColorPicker;
        result.Add(effect);
      }

      return result;

    }

    public static void StartEffect(string typeName, string colorHex)
    {
      var all = GetEffectTypes();

      var effectType = all.Where(x => x.Name == typeName).FirstOrDefault();

      if (effectType != null)
      {
        var hueEffectAtt = effectType.GetCustomAttribute<HueEffectAttribute>();

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

        MethodInfo methodInfo = effectType.GetMethod("Start");

        object result = null;
        object[] parametersArray = new object[] { layer, waitTime, color, cts.Token };
        object classInstance = Activator.CreateInstance(effectType, null);
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

      //Get all effects that inherit from BaseEffect
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
  }
}
