using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Effects.BasEffects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Layers
{
  [HueEffect(Order = 2, Name = "HSB Loop Effect", HasColorPicker = false)]
  public class HsbLoopEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      HSB hsb = new HSB();
      hsb.Hue = 0;
      hsb.Brightness = 255;
      hsb.Saturation = 255;

      while (!cancellationToken.IsCancellationRequested)
      {
        layer.SetState(cancellationToken, hsb.GetRGB(), 1);

        await Task.Delay(waitTime() / 10);
        hsb.Hue += 100;

        if (hsb.Hue >= 65535)
          hsb.Hue = 0;
      }

    }
  }
}
