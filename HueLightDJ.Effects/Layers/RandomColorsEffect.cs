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
		  [HueEffect(Name = "Random colors (all different)", HasColorPicker = false)]
		  public class RandomColorsEffect : IHueEffect
		  {
					public Task Start(EntertainmentLayer layer, Ref<TimeSpan?> waitTime, RGBColor? color, CancellationToken cancellationToken)
					{
							  return layer.To2DGroup().SetRandomColor(IteratorEffectMode.AllIndividual, IteratorEffectMode.All, waitTime, cancellationToken: cancellationToken);
					}
		  }
}
