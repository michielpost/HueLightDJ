using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Random Pulse top bottom", HasColorPicker = false)]
  public class RandomPulseTopBottomEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      Func<TimeSpan> customWaitTime = () => waitTime() / 10;

      var bottomPulseEffect = new Q42.HueApi.Streaming.Effects.RandomPulseEffect(waitTime: customWaitTime);
      bottomPulseEffect.AutoRepeat = false;
      bottomPulseEffect.Y = -1;
      layer.PlaceEffect(bottomPulseEffect);

      var topPulseEffect = new Q42.HueApi.Streaming.Effects.RandomPulseEffect(waitTime: customWaitTime);
      topPulseEffect.AutoRepeat = false;
      topPulseEffect.Y = 1;
      layer.PlaceEffect(topPulseEffect);

      while(!cancellationToken.IsCancellationRequested)
      {
        bottomPulseEffect.Start();

        await Task.Delay(waitTime() * 2.2);

        topPulseEffect.Start();

        await Task.Delay(waitTime() * 2.2);
      }

      cancellationToken.Register(() =>
      {
        bottomPulseEffect.Stop();
        layer.Effects.Remove(bottomPulseEffect);

        topPulseEffect.Stop();
        layer.Effects.Remove(topPulseEffect);
      });

    }
  }
}
