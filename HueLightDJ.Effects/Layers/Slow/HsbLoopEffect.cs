using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.ColorConverters.HSB;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Effects.BasEffects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Layers
{
  [HueEffect(Order = 2, Name = "HSB Loop Effect", Group = "Slow", HasColorPicker = false)]
  public class HsbLoopEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      HSB hsb = new HSB(0, 255, 255);

      while (!cancellationToken.IsCancellationRequested)
      {
        layer.SetState(cancellationToken, hsb.GetRGB(), 1);

        await Task.Delay(waitTime() / 10);
        hsb.Hue += 100;

        if (hsb.Hue >= HSB.HueMaxValue)
          hsb.Hue = 0;
      }

    }
  }
}
