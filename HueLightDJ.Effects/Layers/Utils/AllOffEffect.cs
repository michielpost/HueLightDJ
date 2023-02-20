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
  [HueEffect(Order = int.MaxValue, Name = "All Off", Group = "Utils", HasColorPicker = false)]
  public class AllOffEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      layer.SetBrightness(cancellationToken, 0, waitTime(), false);

      //Wait for other events to finish and set brightness again
      await Task.Delay(waitTime(), cancellationToken);
      layer.SetBrightness(cancellationToken, 0, waitTime(), false);
    }
  }
}
