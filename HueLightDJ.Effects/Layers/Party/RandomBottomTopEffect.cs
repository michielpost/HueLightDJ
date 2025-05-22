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
  [HueEffect(Order = 3, Name = "Random Bottom Top Effect", Group = "Party", HasColorPicker = false)]
  public class RandomBottomTopEffect : IHueEffect
  {

    protected virtual bool DipToBlack { get; set; } = true;
    protected virtual double DipToBlackWait { get; set; } = 0.8;

    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var r = new Random();
      var center = EffectSettings.LocationCenter;
      var orderedLayer = layer.OrderByDescending(x => x.LightLocation.Angle(center.X, center.Y));

      while (!cancellationToken.IsCancellationRequested)
      {
        color = RGBColor.Random();

        foreach (var light in orderedLayer)
        {
          _ = Task.Run(async () =>
          {
            var distance = 1 + light.LightLocation.Y;
            var timeSpan = waitTime() / 2 * distance;
            await Task.Delay(timeSpan);
            //Debug.WriteLine($"{light.Id} Angle {angle} and timespan {timeSpan.TotalMilliseconds}");
            light.SetState(cancellationToken, color, 1, waitTime() / 2);

            if (DipToBlack)
            {
              await Task.Delay(waitTime() * DipToBlackWait);
              light.SetBrightness(cancellationToken, 0, waitTime() * 0.1);
            }

          });
    
        }

        await Task.Delay(waitTime() * 1.1);
      }

    }
  }
}
