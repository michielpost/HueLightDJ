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
		  [HueEffect(Name = "Random Pulse from center with retrace", HasColorPicker = false)]
		  public class RandomPulseRetraceEffect : IHueEffect
		  {
					public Task Start(EntertainmentLayer layer, Ref<TimeSpan?> waitTime, RGBColor? color, CancellationToken cancellationToken)
					{
							  var customWaitTimeMs = waitTime.Value.Value.TotalMilliseconds / 10;
							  var customWaitTime = TimeSpan.FromMilliseconds(customWaitTimeMs);

							  var randomPulseEffect = new Q42.HueApi.Streaming.Effects.RandomPulseEffect(waitTime: customWaitTime);
							  layer.PlaceEffect(randomPulseEffect);
							  randomPulseEffect.Start();

							  cancellationToken.Register(() => randomPulseEffect.Stop());

							  return Task.CompletedTask;
					}
		  }
}
