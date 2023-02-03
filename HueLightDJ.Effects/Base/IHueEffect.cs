using HueApi.ColorConverters;
using HueApi.Entertainment.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
		  public interface IHueEffect
		  {
					Task Start(EntertainmentLayer layer,
                              Func<TimeSpan> waitTime,
                              RGBColor? color,
                              CancellationToken cancellationToken);
		  }
}
