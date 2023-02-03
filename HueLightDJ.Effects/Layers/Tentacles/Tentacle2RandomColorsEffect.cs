using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Tentacles (2) Random Colors", Group = "Tentacles", HasColorPicker = false)]
  public class Tentacles2RandomColorsEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var center = EffectSettings.LocationCenter;

      var tentacles = layer.GroupBy(x => (int)((x.LightLocation.Angle(center.X, center.Y) / 3.6 / 2))).OrderBy(x => x.Key);

      var grouped = tentacles.ChunkByGroupNumber(2).Select(x => x.SelectMany(l => l));

      return grouped.SetRandomColor(cancellationToken, IteratorEffectMode.AllIndividual, IteratorEffectMode.All, waitTime);
    }
  }
}
