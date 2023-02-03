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

namespace HueLightDJ.Effects
{
  [HueEffect(Order = 5, Name = "Quick Flash", Group = "Flash", DefaultColor = "#FFFFFF")]
  public class QuickFlashEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
        color = RGBColor.Random();

      var fronToBack = layer.GroupBy(x => (int)(((x.LightLocation.Y + 1) / 2) * 50)).ToList();

      Func<TimeSpan> customWaitMS = () => TimeSpan.FromMilliseconds((waitTime().TotalMilliseconds * 2) / fronToBack.Count);

      return fronToBack.FlashQuick(cancellationToken, color, IteratorEffectMode.Bounce, IteratorEffectMode.All, waitTime: customWaitMS);

    }
  }
}
