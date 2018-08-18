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
  [HueEffect(Name = "Flashing Stars back to front", IsBaseEffect = false, HasColorPicker = true, DefaultColor = "#FFFFFF")]
  public class BackFrontFlashEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      //Non repeating effects should not run on baselayer
      if (layer.IsBaseLayer)
        return Task.CompletedTask;

      if (!color.HasValue)
        color = RGBColor.Random();

      Func<TimeSpan> customWaitMS = () => TimeSpan.FromMilliseconds((waitTime().TotalMilliseconds * 2) / layer.Count);

      var fronToBack = layer.GroupBy(x => (int)(((x.LightLocation.Y + 1) / 2) * 50)).Reverse();

      fronToBack.FlashQuick(cancellationToken, color, IteratorEffectMode.Bounce, IteratorEffectMode.RandomOrdered, waitTime: customWaitMS, duration: waitTime());
      fronToBack.FlashQuick(cancellationToken, color, IteratorEffectMode.Bounce, IteratorEffectMode.RandomOrdered, waitTime: customWaitMS, duration: waitTime());

      return Task.CompletedTask;
    }
  }
}
