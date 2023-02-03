using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Order = int.MaxValue, Name = "X-MAS fade", Group = "Utils", HasColorPicker = false)]

  public class ChristmasFadeEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      layer.To2DGroup().Christmas(new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token);

      return layer.To2DGroup().ChristmasFade(waitTime, cancellationToken);
    }

   
  }

  public static class ChristmanExtensions
  {
    public static async Task ChristmasFade(this IEnumerable<IEnumerable<EntertainmentLight>> group, Func<TimeSpan> waitTime, CancellationToken cancellationToken)
    {
      bool startGreen = false;
      while (!cancellationToken.IsCancellationRequested)
      {
        await group.SetChristmas(waitTime, cancellationToken, startGreen).ConfigureAwait(false);
        await Task.Delay(TimeSpan.FromMilliseconds(waitTime().TotalMilliseconds / 4), cancellationToken).ConfigureAwait(false);
        startGreen = !startGreen;
      }
    }

    private static Task SetChristmas(this IEnumerable<IEnumerable<EntertainmentLight>> group, Func<TimeSpan> waitTime, CancellationToken cancellationToken, bool startGreen = false)
    {
      Random r = new Random();
      return group.IteratorEffect(cancellationToken, async (current, ct, timeSpan) =>
      {
        if (startGreen)
          current.SetState(ct, new HueApi.ColorConverters.RGBColor("00FF00"), 1, TimeSpan.FromMilliseconds(waitTime().TotalMilliseconds / 6 * r.Next(1,5)));
        else
          current.SetState(ct, new HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromMilliseconds(waitTime().TotalMilliseconds / 6 * r.Next(1, 5)));

        startGreen = !startGreen;
      }, IteratorEffectMode.RandomOrdered, IteratorEffectMode.RandomOrdered, () => TimeSpan.FromMilliseconds(r.Next(10, 50)));
    }
  }
}
