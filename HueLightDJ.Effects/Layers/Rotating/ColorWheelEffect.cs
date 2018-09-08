using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Effects.BasEffects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Layers
{
  [HueEffect(Order = 3, Name = "Color Wheel Effect", Group = "Rotating", HasColorPicker = false)]
  public class ColorWheelEffect : IHueEffect
  {

    protected virtual int AddRotation  => 9;
    protected virtual int StartRotation { get; set; } = 0;

    public int Chunks { get; set; } = 3;
    private List<RGBColor> _colors = new List<RGBColor>();

    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var r = new Random();
      var center = EffectSettings.LocationCenter;
      var orderedLayer = layer.OrderByDescending(x => x.LightLocation.Angle(center.X, center.Y));

      for (int i = 0; i < Chunks; i++)
      {
        _colors.Add(RGBColor.Random());
      }

      while (!cancellationToken.IsCancellationRequested)
      {

        foreach (var light in orderedLayer)
        {
          Task.Run(() =>
          {
            var angle = light.LightLocation.Angle(center.X, center.Y).Move360(StartRotation);
            double normalAngle = WrapValue(360, (int)angle);

            int arrayIndex = (int)(normalAngle / 361 * Chunks);
            light.SetState(cancellationToken, _colors[arrayIndex], 1);

          });
    
        }

        StartRotation += AddRotation;
        StartRotation = WrapValue(360, StartRotation);
        await Task.Delay(waitTime() / 6);
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
