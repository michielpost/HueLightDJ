using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Group
{
  [HueEffect(Name = "Random colorloop", HasColorPicker = false)]
  public class RandomColorloopEffect : IHueGroupEffect
  {
    public Task Start(IEnumerable<IEnumerable<EntertainmentLight>> layer, Func<TimeSpan> waitTime, RGBColor? color, IteratorEffectMode iteratorMode, IteratorEffectMode secondaryIteratorMode, CancellationToken cancellationToken)
    {
      if (iteratorMode != IteratorEffectMode.All)
      {
        if (secondaryIteratorMode == IteratorEffectMode.Bounce
          || secondaryIteratorMode == IteratorEffectMode.Cycle
          || secondaryIteratorMode == IteratorEffectMode.Random
          || secondaryIteratorMode == IteratorEffectMode.RandomOrdered
          || secondaryIteratorMode == IteratorEffectMode.Single)
        {
          Func<TimeSpan> customWaitMS = () => TimeSpan.FromMilliseconds((waitTime().TotalMilliseconds * layer.Count()) / layer.SelectMany(x => x).Count());

          return layer.SetRandomColor(cancellationToken, iteratorMode, secondaryIteratorMode, customWaitMS, customWaitMS);
        }
      }

      return layer.SetRandomColor(cancellationToken, iteratorMode, secondaryIteratorMode, waitTime, waitTime);
    }
  }
}
