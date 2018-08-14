using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Touch
{
  [HueEffect(Name = "Ripple Touch", IsBaseEffect = false, HasColorPicker = false)]
  class RippleTouchEffect : IHueTouchEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken, double x, double y)
    {
      Func<TimeSpan> customWaitTime = () => waitTime() / 10;

      Q42.HueApi.Streaming.Effects.RandomPulseEffect effect = new Q42.HueApi.Streaming.Effects.RandomPulseEffect(false, customWaitTime);
      effect.X = x;
      effect.Y = y;
      effect.AutoRepeat = false;
      layer.PlaceEffect(effect);
      effect.Start();

      return Task.Run(async () =>
      {
        await Task.Delay(waitTime() * 2);
        effect.Stop();
        layer.Effects.Remove(effect);
      });
    }
  }
}
