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
          var addHue = r.Next(-6000, 6000);
          var addBri = r.Next(-100, 100);
          var randomHsb = new HSB(hsb.Hue + addHue, hsb.Saturation, WrapValue(255, hsb.Brightness + addBri));
          light.SetState(cancellationToken, randomHsb.GetRGB(), 1);
        }
        await Task.Delay(waitTime());
      }
    }

    private int WrapValue(int max, int value)
    {
      var result = ((value % max) + max) % max;

      //At least 50, to avoid dark/off lights
      if (result < 50)
        result += 50;

      return result;
    }
  }
}
