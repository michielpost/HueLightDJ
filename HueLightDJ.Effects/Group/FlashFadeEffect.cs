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
  [HueEffect(Name = "Flash and fade")]
  public class FlashFadeEffect : IHueGroupEffect
  {
    public Task Start(IEnumerable<IEnumerable<EntertainmentLight>> layer, Ref<TimeSpan?> waitTime, RGBColor? color, IteratorEffectMode iteratorMode, IteratorEffectMode secondaryIteratorMode, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
      {
        var r = new Random();
        color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
      }

      if (iteratorMode != IteratorEffectMode.All)
      {
        if(secondaryIteratorMode == IteratorEffectMode.Bounce
          || secondaryIteratorMode == IteratorEffectMode.Random
          || secondaryIteratorMode == IteratorEffectMode.Single)
        {
          var customWaitMS = (waitTime.Value.Value.TotalMilliseconds * 2) / layer.SelectMany(x => x).Count();

          return layer.FlashQuick(cancellationToken, color, iteratorMode, secondaryIteratorMode, waitTime: TimeSpan.FromMilliseconds(customWaitMS), transitionTimeOn: TimeSpan.Zero, transitionTimeOff: waitTime);
        }
      }

      return layer.FlashQuick(cancellationToken, color, iteratorMode, secondaryIteratorMode, waitTime: waitTime, transitionTimeOn: TimeSpan.Zero, transitionTimeOff: waitTime);
    }
  }
}
