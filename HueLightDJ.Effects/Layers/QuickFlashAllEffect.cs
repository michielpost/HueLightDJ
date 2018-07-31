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
  [HueEffect(Name = "Quick Flash on all lights", DefaultColor = "#FFFFFF")]
  public class QuickFlashAllEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
      {
        var r = new Random();
        color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
      }

      return layer.To2DGroup().FlashQuick(cancellationToken, color, IteratorEffectMode.All, waitTime: waitTime);
    }
  }
}
