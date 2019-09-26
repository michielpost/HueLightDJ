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
