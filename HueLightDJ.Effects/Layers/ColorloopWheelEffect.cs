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
  [HueEffect(Name = "Colorloop Wheel Effect", HasColorPicker = false)]
  public class ColorloopWheelEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var r = new Random();
      var orderedLayer = layer.OrderByDescending(x => x.LightLocation.Angle(0, 0) + 180);

      while (!cancellationToken.IsCancellationRequested)
      {
        var randomColor = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());

        foreach (var light in orderedLayer)
        {
          Task.Run(async () =>
          {
            var angle = light.LightLocation.Angle(0, 0) + 180;
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
