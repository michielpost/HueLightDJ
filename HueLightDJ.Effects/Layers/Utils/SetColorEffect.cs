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
  [HueEffect(Order = int.MaxValue, Name = "Set Color", Group = "Utils", HasColorPicker = true)]
  public class SetColorEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
        color = RGBColor.Random();

      layer.SetState(cancellationToken, color, 1, waitTime(), false);

      //Wait for other events to finish and set state again
      await Task.Delay(waitTime(), cancellationToken);
      layer.SetState(cancellationToken, color, 1, waitTime(), false);
    }
  }
}
