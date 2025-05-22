using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.ColorConverters.HSB;
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
  [HueEffect(Order = 3, Name = "Rainbow Wheel Effect", Group = "Rotating", HasColorPicker = false)]
  public class RainbowWheelEffect : IHueEffect
  {

    protected virtual int Steps  => (int)(HSB.HueMaxValue * 0.85);
    protected virtual int StartStep { get; set; } = 0;
    protected virtual bool DipToBlack { get; set; } = true;

    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var r = new Random();
      var center = EffectSettings.LocationCenter;
      var orderedLayer = layer.OrderByDescending(x => x.LightLocation.Angle(center.X, center.Y));

      while (!cancellationToken.IsCancellationRequested)
      {

        foreach (var light in orderedLayer)
        {
          _ = Task.Run(async () =>
          {
            var angle = light.LightLocation.Angle(center.X, center.Y).Move360(90);
            var timeSpan = waitTime() / 360 * angle;
            var addHue = (int)(Steps / 360 * angle);
            await Task.Delay(timeSpan);
            //Debug.WriteLine($"{light.Id} Angle {angle} and timespan {timeSpan.TotalMilliseconds}");
            var hsbColor = new HSB(StartStep + addHue, 255, 255);
            light.SetState(cancellationToken, hsbColor.GetRGB(), 1, waitTime() / 2);

            if (DipToBlack)
            {
              await Task.Delay(waitTime() * 0.8);
              light.SetBrightness(cancellationToken, 0, waitTime() * 0.1);
            }

          });
    
        }

        StartStep += Steps;
        await Task.Delay(waitTime() * 1.1);
      }

    }
  }
}
