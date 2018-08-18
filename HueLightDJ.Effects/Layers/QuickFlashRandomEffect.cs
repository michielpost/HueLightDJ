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

namespace HueLightDJ.Effects
{
  [HueEffect(Order = 6, Name = "Quick Random Flash", DefaultColor = "#FFFFFF")]
  public class QuickFlashRandomEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
        color = RGBColor.Random();

      var groupCount = layer.Count / 3;

      Func<TimeSpan> customWaitMS = () => TimeSpan.FromMilliseconds((waitTime().TotalMilliseconds * 2) / groupCount);

      return layer.OrderBy(x => Guid.NewGuid()).ChunkByGroupNumber(groupCount).FlashQuick(cancellationToken, color, IteratorEffectMode.Random, waitTime: customWaitMS);
    }
  }
}
