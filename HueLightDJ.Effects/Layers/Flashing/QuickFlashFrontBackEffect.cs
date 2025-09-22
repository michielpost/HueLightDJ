using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using HueLightDJ.Effects.Base;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Order = 6, Name = "Quick Back to Front Flash", Group = "Flash", DefaultColor = "#FFFFFF")]
  public class QuickFlashFrontBackEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
        color = RGBColor.Random();

      Func<TimeSpan> customWaitMS = () => TimeSpan.FromMilliseconds((waitTime().TotalMilliseconds * 4) / layer.Count);

      var fronToBack = layer.GroupBy(x => (int)(((x.LightLocation.Y + 1) / 2) * 50)).OrderBy(x => x.Key);

      fronToBack.FlashQuick(cancellationToken, color, IteratorEffectMode.Bounce, IteratorEffectMode.RandomOrdered, waitTime: customWaitMS);
      fronToBack.FlashQuick(cancellationToken, color, IteratorEffectMode.Bounce, IteratorEffectMode.RandomOrdered, waitTime: customWaitMS);

      return Task.CompletedTask;
    }
  }
}
