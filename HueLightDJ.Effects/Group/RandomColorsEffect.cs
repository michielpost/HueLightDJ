using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Group
{
		  [HueEffect(Name = "Random colors (all different)", HasColorPicker = false)]
		  public class RandomColorsEffect : IHueGroupEffect
  {
					public Task Start(IEnumerable<IEnumerable<EntertainmentLight>> layer, Ref<TimeSpan?> waitTime, RGBColor? color, IteratorEffectMode iteratorMode, IteratorEffectMode secondaryIteratorMode, CancellationToken cancellationToken)
					{
							  return layer.SetRandomColor(iteratorMode, waitTime, cancellationToken: cancellationToken);
					}
		  }
}
