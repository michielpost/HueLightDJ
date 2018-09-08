using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Order = 1, Name = "Circle Light", Group = "Rotating")]
  public class CircleLightEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
        color = RGBColor.Random();

      var center = EffectSettings.LocationCenter;
      var orderedByAngle = layer.OrderBy(x => x.LightLocation.Angle(center.X, center.Y));

      Func<TimeSpan> customWaitMS = () => TimeSpan.FromMilliseconds((waitTime().TotalMilliseconds * 2) / layer.Count);
      Func<TimeSpan> customOnTime = () => customWaitMS() / 2;
      Func<TimeSpan> customOffTime = () => customWaitMS() * 2;

      return orderedByAngle.To2DGroup().Flash(cancellationToken, color, IteratorEffectMode.Cycle, waitTime: customWaitMS, transitionTimeOn: customOnTime, transitionTimeOff: customOffTime, waitTillFinished: false);

    }
  }
}
