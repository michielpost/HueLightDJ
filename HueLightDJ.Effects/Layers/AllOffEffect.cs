using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Order = int.MaxValue, Name = "All Off", HasColorPicker = false)]
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
