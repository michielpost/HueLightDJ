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

namespace HueLightDJ.Effects.Group
{
  [HueEffect(Name = "Quick Flash")]
  public class QuickFlashEffect : IHueGroupEffect
  {
    public Task Start(IEnumerable<IEnumerable<EntertainmentLight>> layer, Ref<TimeSpan?> waitTime, RGBColor? color, IteratorEffectMode iteratorMode, IteratorEffectMode secondaryIteratorMode, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
        color = new Q42.HueApi.ColorConverters.RGBColor("FFFFFF");

      //var customWaitMS = (waitTime.Value.Value.TotalMilliseconds * 2) / layer.Count();

      return layer.FlashQuick(color, iteratorMode, secondaryIteratorMode, waitTime: waitTime, cancellationToken: cancellationToken);
    }
  }
}
