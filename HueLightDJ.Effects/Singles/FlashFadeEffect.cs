using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Flash and fade", IsBaseEffect = false, HasColorPicker = true)]
  public class FlashFadeEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Ref<TimeSpan?> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      //Non repeating effects should not run on baselayer
      if (layer.IsBaseLayer)
        return Task.CompletedTask;

      if (!color.HasValue)
        color = new Q42.HueApi.ColorConverters.RGBColor("FFFFFF");

      return layer.To2DGroup().FlashQuick(cancellationToken, color, IteratorEffectMode.All, IteratorEffectMode.All, waitTime: TimeSpan.FromMilliseconds(100), transitionTimeOn: TimeSpan.Zero, transitionTimeOff: waitTime, duration: waitTime);
    }
  }
}
