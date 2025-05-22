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
  [HueEffect(Order = 3, Name = "Colorloop Wheel Effect", Group = "Rotating", HasColorPicker = false)]
  public class ColorloopWheelEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var r = new Random();
      var center = EffectSettings.LocationCenter;
      var orderedLayer = layer.OrderByDescending(x => x.LightLocation.Angle(center.X, center.Y));

      while (!cancellationToken.IsCancellationRequested)
      {
        var randomColor = RGBColor.Random(r);

        foreach (var light in orderedLayer)
        {
          _ = Task.Run(async () =>
          {
            var angle = light.LightLocation.Angle(center.X, center.Y).Move360(90);
            var timeSpan = waitTime() / 360 * angle;
            await Task.Delay(timeSpan);
            //Debug.WriteLine($"{light.Id} Angle {angle} and timespan {timeSpan.TotalMilliseconds}");
            light.SetState(cancellationToken, randomColor, 1, waitTime() / 2);
          });
    
        }

        await Task.Delay(waitTime() * 1.1);
      }

    }
  }
}
