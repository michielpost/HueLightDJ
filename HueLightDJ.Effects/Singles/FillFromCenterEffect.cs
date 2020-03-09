using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Fill from center", IsBaseEffect = false)]
  public class FillFromCenterEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      //Non repeating effects should not run on baselayer
      if (layer.IsBaseLayer)
        return;

      var center = EffectSettings.LocationCenter;
      var orderedByDistance = layer.OrderBy(x => x.LightLocation.Distance(center.X, center.Y, center.Z));

      if (!color.HasValue)
        color = RGBColor.Random();

      Func<TimeSpan> customWaitTime = () => waitTime() / layer.Count;


      await orderedByDistance.To2DGroup().SetColor(cancellationToken, color.Value, IteratorEffectMode.Single, IteratorEffectMode.All, customWaitTime);
      layer.SetBrightness(cancellationToken, 0, transitionTime: TimeSpan.FromMilliseconds(0));
    }
  }
}
