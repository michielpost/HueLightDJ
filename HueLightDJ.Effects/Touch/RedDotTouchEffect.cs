using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HueApi.Entertainment.Effects.Examples;

namespace HueLightDJ.Effects.Touch
{
  //Boring example, do not use
  //[HueEffect(Name = "Red Dot Touch", IsBaseEffect = false, HasColorPicker = false)]
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
        await Task.Delay(waitTime());
        effect.Stop();
        layer.Effects.Remove(effect);
      });
    }
  }
}
