using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Effects.BasEffects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Layers
{
  [HueEffect(Name = "Colorloop Effect", HasColorPicker = false)]
  public class ColorloopEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {

      Func<TimeSpan> customWaitTime = () => waitTime();

      layer.To2DGroup().SetRandomColor(cancellationToken, IteratorEffectMode.All, IteratorEffectMode.All, waitTime, waitTime, null);

      //var r = new Random();

      //while (!cancellationToken.IsCancellationRequested)
      //{
      //  var randomColor = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());


      //  await Task.Delay(waitTime());
      //}

    }
  }
}
