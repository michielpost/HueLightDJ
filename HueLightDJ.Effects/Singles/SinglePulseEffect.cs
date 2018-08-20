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
  [HueEffect(Name = "Single random pulse from center", IsBaseEffect = false, HasColorPicker = false)]
  public class SinglePulseEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      //Non repeating effects should not run on baselayer
      if (layer.IsBaseLayer)
        return;

      Func<TimeSpan> customWaitTime = () => waitTime() / 10;

      var center = EffectSettings.LocationCenter;
      var randomPulseEffect = new Q42.HueApi.Streaming.Effects.RandomPulseEffect(fadeToZero: false, waitTime: customWaitTime);
      randomPulseEffect.X = center.X;
      randomPulseEffect.Y = center.Y;
      cancellationToken.Register(() => {
        try
        {
          randomPulseEffect.Stop();
        }
        catch { }
        layer.Effects.Remove(randomPulseEffect);
      });

      layer.PlaceEffect(randomPulseEffect);
      randomPulseEffect.AutoRepeat = false;
      randomPulseEffect.Start();

      await Task.Delay(waitTime() * 2, cancellationToken);
      randomPulseEffect.Stop();
    }
  }
}
