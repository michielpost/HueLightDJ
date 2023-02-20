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
  [HueEffect(Name = "Trailing Light")]
  public class TrailingLightEffect : IHueGroupEffect
  {
    public Task Start(IEnumerable<IEnumerable<EntertainmentLight>> layer, Func<TimeSpan> waitTime, RGBColor? color, IteratorEffectMode iteratorMode, IteratorEffectMode secondaryIteratorMode, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
        color = RGBColor.Random();

      if (iteratorMode == IteratorEffectMode.All)
        iteratorMode = IteratorEffectMode.AllIndividual;

      //This is no fun, no action, change it to cycle
      if (iteratorMode == IteratorEffectMode.AllIndividual
        && (secondaryIteratorMode == IteratorEffectMode.All || secondaryIteratorMode == IteratorEffectMode.AllIndividual))
        secondaryIteratorMode = IteratorEffectMode.Cycle;

      if (iteratorMode != IteratorEffectMode.AllIndividual)
      {
        if (secondaryIteratorMode == IteratorEffectMode.Bounce
          || secondaryIteratorMode == IteratorEffectMode.Cycle
          || secondaryIteratorMode == IteratorEffectMode.Random
          || secondaryIteratorMode == IteratorEffectMode.RandomOrdered
          || secondaryIteratorMode == IteratorEffectMode.Single)
        {
          Func<TimeSpan> customWaitMS = () => TimeSpan.FromMilliseconds((waitTime().TotalMilliseconds * layer.Count()) / layer.SelectMany(x => x).Count());
          Func<TimeSpan> customOnTime = () => customWaitMS() / 2;
          Func<TimeSpan> customOffTime = () => customWaitMS() * 2;

          return layer.Flash(cancellationToken, color, iteratorMode, secondaryIteratorMode, waitTime: customWaitMS, transitionTimeOn: customOnTime, transitionTimeOff: customOffTime, waitTillFinished: false);
        }
      }

      Func<TimeSpan> onTime = () => waitTime() / 2;
      Func<TimeSpan> offTime = () => waitTime() * 2;

      return layer.Flash(cancellationToken, color, iteratorMode, secondaryIteratorMode, waitTime: waitTime, transitionTimeOn: onTime, transitionTimeOff: offTime, waitTillFinished: false);

    }
  }
}
