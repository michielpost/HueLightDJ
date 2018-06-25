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
		  [HueEffect(Name = "Single random pulse from center", IsBaseEffect = false)]
		  public class SinglePulseEffect : IHueEffect
		  {
					public async Task Start(EntertainmentLayer layer, Ref<TimeSpan?> waitTime, RGBColor? color, CancellationToken cancellationToken)
					{
							  //Non repeating effects should not run on baselayer
							  if (layer.IsBaseLayer)
										return;

							  var customWaitTimeMs = waitTime.Value.Value.TotalMilliseconds / 10;
							  var customWaitTime = TimeSpan.FromMilliseconds(customWaitTimeMs);

							  var randomPulseEffect = new Q42.HueApi.Streaming.Effects.RandomPulseEffect(fadeToZero: false, waitTime: customWaitTime);
							  layer.PlaceEffect(randomPulseEffect);
							  randomPulseEffect.Start();
							  cancellationToken.Register(() => randomPulseEffect.Stop());

							  await Task.Delay(waitTime.Value.Value);
							  randomPulseEffect.Stop();

							  //TODO: Delete after NuGet package update with new RandomPulseEffect
							  layer.SetBrightness(0, transitionTime: TimeSpan.FromMilliseconds(0));
					}
		  }
}
