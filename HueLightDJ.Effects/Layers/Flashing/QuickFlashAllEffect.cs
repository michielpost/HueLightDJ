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
  [HueEffect(Order = 4, Name = "Quick Flash on all lights", Group = "Flash", DefaultColor = "#FFFFFF")]
  public class QuickFlashAllEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
        color = RGBColor.Random();

      return layer.To2DGroup().FlashQuick(cancellationToken, color, IteratorEffectMode.All, waitTime: waitTime);
    }
  }
}
