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
		  [HueEffect(Order = int.MaxValue, Name = "X-MAS", Group = "Utils", HasColorPicker = false)]

          public class ChristmasEffect : IHueEffect
		  {
					public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
					{
							  return layer.To2DGroup().Christmas(cancellationToken);
					}
		  }
}
