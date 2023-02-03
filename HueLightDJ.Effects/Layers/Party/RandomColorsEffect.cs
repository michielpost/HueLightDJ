using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
		  [HueEffect(Name = "Random colors (all different)", Group = "Party", HasColorPicker = false)]
		  public class RandomColorsEffect : IHueEffect
		  {
					public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
					{
							  return layer.To2DGroup().SetRandomColor(cancellationToken, IteratorEffectMode.AllIndividual, IteratorEffectMode.All, waitTime);
					}
		  }
}
