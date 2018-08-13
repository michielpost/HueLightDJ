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
  [HueEffect(Name = "Red Dot Touch", IsBaseEffect = false, HasColorPicker = false)]
  class RedDotTouchEffect : IHueTouchEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken, double x, double y)
    {
      RedLightEffect effect = new RedLightEffect();
      effect.X = x;
      effect.Y = y;
      layer.PlaceEffect(effect);
      effect.Start();

      return Task.Run(async () =>
      {
        await Task.Delay(TimeSpan.FromSeconds(1));
        effect.Stop();
      });
    }
  }
}
