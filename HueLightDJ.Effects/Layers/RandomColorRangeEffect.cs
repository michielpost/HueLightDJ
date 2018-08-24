using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Random color range", HasColorPicker = false)]
  public class RandomColorRangeEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      Random r = new Random();
      while(!cancellationToken.IsCancellationRequested)
      {
        color = RGBColor.Random();
        var hsb = color.Value.GetHSB();

        foreach(var light in layer)
        {
          var add = r.Next(-6000, 6000);
          var randomHsb = new HSB(hsb.Hue + add, 255, 255);
          light.SetState(cancellationToken, randomHsb.GetRGB(), 1);
        }
        await Task.Delay(waitTime());
      }
    }
  }
}
