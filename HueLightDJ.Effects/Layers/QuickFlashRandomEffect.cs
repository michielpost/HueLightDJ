using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Quick Random Flash")]
  public class QuickFlashRandomEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Ref<TimeSpan?> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
        color = new Q42.HueApi.ColorConverters.RGBColor("FFFFFF");

      var customWaitMS = (waitTime.Value.Value.TotalMilliseconds * 2) / layer.Count;

      return layer.To2DGroup().FlashQuick(cancellationToken, color, IteratorEffectMode.Random, waitTime: TimeSpan.FromMilliseconds(customWaitMS));
    }
  }
}
