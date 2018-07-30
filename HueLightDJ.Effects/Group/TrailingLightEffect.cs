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

      if (iteratorMode == IteratorEffectMode.All)
        iteratorMode = IteratorEffectMode.AllIndividual;

      if (iteratorMode != IteratorEffectMode.AllIndividual)
      {
        if (secondaryIteratorMode == IteratorEffectMode.Bounce
          || secondaryIteratorMode == IteratorEffectMode.Cycle
          || secondaryIteratorMode == IteratorEffectMode.Random
          || secondaryIteratorMode == IteratorEffectMode.RandomOrdered
          || secondaryIteratorMode == IteratorEffectMode.Single)
        {
          var customWaitMS = (waitTime.Value.Value.TotalMilliseconds * layer.Count()) / layer.SelectMany(x => x).Count();
          var customTimeSpan = TimeSpan.FromMilliseconds(customWaitMS);

          return layer.Flash(cancellationToken, color, iteratorMode, secondaryIteratorMode, waitTime: customTimeSpan, transitionTimeOn: TimeSpan.FromMilliseconds(customWaitMS / 2), transitionTimeOff: TimeSpan.FromMilliseconds(customWaitMS * 2), waitTillFinished: false);
        }
      }

      return layer.Flash(cancellationToken, color, iteratorMode, secondaryIteratorMode, waitTime: waitTime, transitionTimeOn: TimeSpan.FromMilliseconds(waitTime.Value.Value.TotalMilliseconds / 2), transitionTimeOff: TimeSpan.FromMilliseconds(waitTime.Value.Value.TotalMilliseconds * 2), waitTillFinished: false);

    }
  }
}
