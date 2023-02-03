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
  [HueEffect(Name = "Flashing Stars back to front", IsBaseEffect = false, HasColorPicker = true, DefaultColor = "#FFFFFF")]
  public class BackFrontFlashEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      //Non repeating effects should not run on baselayer
      if (layer.IsBaseLayer)
        return Task.CompletedTask;

      if (!color.HasValue)
        color = RGBColor.Random();

      Func<TimeSpan> customWaitMS = () => TimeSpan.FromMilliseconds((waitTime().TotalMilliseconds * 4) / layer.Count);

      var fronToBack = layer.GroupBy(x => (int)(((x.LightLocation.Y + 1) / 2) * 50)).OrderBy(x => x.Key).Reverse();

      return fronToBack.FlashQuick(cancellationToken, color, IteratorEffectMode.Single, IteratorEffectMode.RandomOrdered, waitTime: customWaitMS);
    }
  }
}
