using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Trailing Light")]
  public class TrailingLightEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Ref<TimeSpan?> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
      {
        var r = new Random();
        color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
      }

      var allLightsOrdered = layer.OrderBy(x => x.LightLocation.X).ThenBy(x => x.LightLocation.Y).ToList();

      return allLightsOrdered.Flash(color, IteratorEffectMode.Cycle, waitTime: waitTime, transitionTimeOn: TimeSpan.FromMilliseconds(waitTime.Value.Value.TotalMilliseconds / 2), transitionTimeOff: TimeSpan.FromMilliseconds(waitTime.Value.Value.TotalMilliseconds * 2), waitTillFinished: false, cancellationToken: cancellationToken);

    }
  }
}
