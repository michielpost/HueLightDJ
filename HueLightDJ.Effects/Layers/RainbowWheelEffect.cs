using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Effects.BasEffects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Layers
{
  [HueEffect(Order = 3, Name = "Rainbow Wheel Effect", HasColorPicker = false)]
  public class RainbowWheelEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var r = new Random();
      var center = EffectSettings.LocationCenter;
      var orderedLayer = layer.OrderByDescending(x => x.LightLocation.Angle(center.X, center.Y));

      int steps = (int)(65535 * 0.85);
      int startStep = 0;

      while (!cancellationToken.IsCancellationRequested)
      {

        foreach (var light in orderedLayer)
        {
          Task.Run(async () =>
          {
            var angle = light.LightLocation.Angle(center.X, center.Y).Move360(90);
            var timeSpan = waitTime() / 360 * angle;
            var addHue = (int)(steps / 360 * angle);
            await Task.Delay(timeSpan);
            //Debug.WriteLine($"{light.Id} Angle {angle} and timespan {timeSpan.TotalMilliseconds}");
            var hsbColor = new HSB() { Brightness = 255, Saturation = 255, Hue = GetHueValue(startStep + addHue) };
            light.SetState(cancellationToken, hsbColor.GetRGB(), 1, waitTime() / 2);

            await Task.Delay(waitTime() * 0.8);
            light.SetBrightness(cancellationToken,0, waitTime() * 0.1);

          });
    
        }
        startStep += steps;
        await Task.Delay(waitTime() * 1.1);
      }

    }

    private static int GetHueValue(int value)
    {
      if (value >= 65535)
      {
        double sectorPos = (double)value / 65535;
        int sectorNumber = (int)(Math.Floor(sectorPos));
        // get the fractional part of the sector
        return (int)((sectorPos - sectorNumber) * 65535);
      }
      else
        return (int)value;
    }
  }
}
