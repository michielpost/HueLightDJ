using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
		  [HueEffect(Name = "Fill from center", IsBaseEffect = false)]
		  public class FillFromCenterEffect : IHueEffect
		  {
					public async Task Start(EntertainmentLayer layer, Ref<TimeSpan?> waitTime, RGBColor? color, CancellationToken cancellationToken)
					{
                //Non repeating effects should not run on baselayer
							  if (layer.IsBaseLayer)
										return;

							  var orderedByDistance = layer.OrderBy(x => x.LightLocation.Distance(0, 0));

							  if (!color.HasValue)
							  {
										var r = new Random();
										color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
							  }

							  var customWaitTimeMs = waitTime.Value.Value.TotalMilliseconds / layer.Count;
							  var customWaitTime = TimeSpan.FromMilliseconds(customWaitTimeMs);


							  await orderedByDistance.To2DGroup().SetColor(color.Value, IteratorEffectMode.Single, customWaitTime, cancellationToken: cancellationToken);
							  layer.SetBrightness(0, transitionTime: TimeSpan.FromMilliseconds(0));
					}
		  }
}
