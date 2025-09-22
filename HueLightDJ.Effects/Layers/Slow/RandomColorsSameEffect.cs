using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using HueLightDJ.Effects.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Random colors (all the same)", Group = "Slow", HasColorPicker = false)]
  public class RandomColorsSameEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      return layer.To2DGroup().SetRandomColor(cancellationToken, IteratorEffectMode.All, IteratorEffectMode.All, waitTime);
    }
  }
}
