using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects.BasEffects;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Layers
{
  [HueEffect(Name = "Rotating Effect", HasColorPicker = false)]
  public class RotatingEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Ref<TimeSpan?> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var customWaitTimeMs = waitTime.Value.Value.TotalMilliseconds / 10;
      var customWaitTime = TimeSpan.FromMilliseconds(customWaitTimeMs);

      if(!color.HasValue)
      {
        var r = new Random();
        color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
      }

      var rotatingEffect = new RotatingLineEffect(cancellationToken, color.Value, customWaitTime);
      layer.PlaceEffect(rotatingEffect);
      rotatingEffect.Start();

      cancellationToken.Register(() => rotatingEffect.Stop());

      return Task.CompletedTask;
    }
  }

  public class RotatingLineEffect : AngleEffect
  {
    private CancellationToken _ct;
    private RGBColor _color;
    private Ref<TimeSpan?> _waitTime;

    public RotatingLineEffect(CancellationToken ct, RGBColor color, Ref<TimeSpan?> waitTime)
    {
      Width = 1;
      X = 0;
      Y = 0;

      _ct = ct;
      _color = color;
      _waitTime = waitTime;
    }

    public override void Start()
    {
      base.Start();

      CurrentAngle = 90;

      var state = new Q42.HueApi.Streaming.Models.StreamingState();
      state.SetBrightnes(1);
      state.SetRGBColor(_color);

      this.State = state;

      Rotate(_ct, waitTime: _waitTime);
    }
  }
}
