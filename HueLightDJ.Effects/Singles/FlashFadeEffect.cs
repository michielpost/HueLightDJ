using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using HueLightDJ.Effects.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Flash and fade", IsBaseEffect = false, HasColorPicker = true, DefaultColor = "#FFFFFF")]
  public class FlashFadeEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      //Non repeating effects should not run on baselayer
      if (layer.IsBaseLayer)
        return Task.CompletedTask;

      if (!color.HasValue)
        color = RGBColor.Random();

      return layer.To2DGroup().FlashQuick(cancellationToken, color, IteratorEffectMode.All, IteratorEffectMode.All, waitTime: () => TimeSpan.FromMilliseconds(100), transitionTimeOn: () => TimeSpan.Zero, transitionTimeOff: waitTime, duration: waitTime());
    }
  }
}
