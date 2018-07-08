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

namespace HueLightDJ.Effects.Group
{
  [HueEffect(Name = "Trailing Light")]
  public class TrailingLightEffect : IHueGroupEffect
  {
    public Task Start(IEnumerable<IEnumerable<EntertainmentLight>> layer, Ref<TimeSpan?> waitTime, RGBColor? color, IteratorEffectMode iteratorMode, IteratorEffectMode secondaryIteratorMode, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
      {
        var r = new Random();
        color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
      }

      return layer.Flash(color, iteratorMode, waitTime: waitTime, transitionTimeOn: TimeSpan.FromMilliseconds(waitTime.Value.Value.TotalMilliseconds / 2), transitionTimeOff: TimeSpan.FromMilliseconds(waitTime.Value.Value.TotalMilliseconds * 2), waitTillFinished: false, cancellationToken: cancellationToken);

    }
  }
}
