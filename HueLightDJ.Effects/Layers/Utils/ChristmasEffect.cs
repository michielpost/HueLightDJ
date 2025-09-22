using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Models;
using HueLightDJ.Effects.Base;
using System;
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
