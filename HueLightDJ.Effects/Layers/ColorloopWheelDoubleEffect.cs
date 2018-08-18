using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
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
  [HueEffect(Order = 3, Name = "Colorloop Double Wheel Effect", HasColorPicker = false)]
  public class ColorloopWheelDoubleEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var r = new Random();
      var orderedLeftLayer = layer.OrderByDescending(x => x.LightLocation.Angle(0, 0)).GetLeft();
      var ordereRightLayer = layer.OrderByDescending(x => x.LightLocation.Angle(0, 0)).GetRight();
      var toDark = r.NextDouble() >= 0.5;

      while (!cancellationToken.IsCancellationRequested)
      {
        var randomLeftColor = RGBColor.Random(r);
        var randomRightColor = RGBColor.Random(r);

        foreach (var light in orderedLeftLayer)
        {
          Task.Run(async () =>
          {
            var angle = light.LightLocation.Angle(0, 0).Move360(91);
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
          Task.Run(async () =>
          {
            var angle = Math.Abs(light.LightLocation.Angle(0, 0).Move360(271) - 180);
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
