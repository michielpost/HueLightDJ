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
  [HueEffect(Name = "Random Pulse from center", HasColorPicker = false)]
  public class RandomPulseEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      Func<TimeSpan> customWaitTime = () => waitTime() / 10;

      var randomPulseEffect = new Q42.HueApi.Streaming.Effects.RandomPulseEffect(fadeToZero: false, waitTime: customWaitTime);
      layer.PlaceEffect(randomPulseEffect);
      randomPulseEffect.Start();

      cancellationToken.Register(() =>
      {
        randomPulseEffect.Stop();
        layer.Effects.Remove(randomPulseEffect);
      });

      return Task.CompletedTask;
    }
  }
}
