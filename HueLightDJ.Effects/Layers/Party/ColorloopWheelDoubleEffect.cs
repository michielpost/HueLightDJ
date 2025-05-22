using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Effects.BasEffects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Layers
{
  [HueEffect(Order = 3, Name = "Colorloop Double Wheel Effect", Group = "Party", HasColorPicker = false)]
  public class ColorloopWheelDoubleEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var r = new Random();
      var center = EffectSettings.LocationCenter;
      var orderedLeftLayer = layer.OrderByDescending(x => x.LightLocation.Angle(center.X, center.Y)).GetLeft();
      var ordereRightLayer = layer.OrderByDescending(x => x.LightLocation.Angle(center.X, center.Y)).GetRight();
      var toDark = r.NextDouble() >= 0.5;

      while (!cancellationToken.IsCancellationRequested)
      {
        var randomLeftColor = RGBColor.Random(r);
        var randomRightColor = RGBColor.Random(r);

        foreach (var light in orderedLeftLayer)
        {
          _ = Task.Run(async () =>
          {
            var angle = light.LightLocation.Angle(center.X, center.Y).Move360(91);
            var timeSpan = waitTime() / 360 * angle;
            await Task.Delay(timeSpan);
            //Debug.WriteLine($"{light.Id} Angle {angle} and timespan {timeSpan.TotalMilliseconds}");
            light.SetState(cancellationToken, randomLeftColor, 1, waitTime() / 2);

            if (toDark)
            {
              await Task.Delay(waitTime() * 1.1);
              await Task.Delay(timeSpan);
              light.SetBrightness(cancellationToken, 0, waitTime() / 2);
            }
          });
    
        }

        await Task.Delay(waitTime() * 1.1);

        foreach (var light in ordereRightLayer)
        {
          _ = Task.Run(async () =>
          {
            var angle = Math.Abs(light.LightLocation.Angle(center.X, center.Y).Move360(271) - 180);
            var timeSpan = waitTime() / 360 * angle;
            await Task.Delay(timeSpan);
            //Debug.WriteLine($"{light.Id} Angle {angle} and timespan {timeSpan.TotalMilliseconds}");
            light.SetState(cancellationToken, randomRightColor, 1, waitTime() / 2);

            if (toDark)
            {
              await Task.Delay(waitTime() * 1.1);
              await Task.Delay(timeSpan);
              light.SetBrightness(cancellationToken, 0, waitTime() / 2);
            }
          });

        }
       
        await Task.Delay(waitTime() * 1.1);
      }

    }
  }
}
