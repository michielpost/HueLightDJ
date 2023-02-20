using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Effects.BasEffects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Layers
{
  [HueEffect(Order = 2, Name = "Colorloop Effect", Group = "Slow", HasColorPicker = false)]
  public class ColorloopEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      Func<TimeSpan> customWaitTime = () => waitTime();

      return layer.To2DGroup().SetRandomColor(cancellationToken, IteratorEffectMode.All, IteratorEffectMode.All, waitTime, waitTime, null);
    }
  }
}
