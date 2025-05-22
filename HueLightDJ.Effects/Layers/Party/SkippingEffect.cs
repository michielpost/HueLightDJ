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
  [HueEffect(Name = "Skipping Effect", Group = "Party", HasColorPicker = false, Order = 1)]
  public class SkippingEffect : IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var allLightsOrdered = layer.OrderBy(x => x.LightLocation.X).ThenBy(x => x.LightLocation.Y).ToList();

      var leftToRight = allLightsOrdered.GroupBy(x => (int)(((x.LightLocation.X + 1) / 2) * 50)).Select(x => x.Select(s => s)).ToList();
      var effectGroups = leftToRight.ChunkByGroupNumber(2).ToList();

      while (!cancellationToken.IsCancellationRequested)
      {
        bool skip = false;
        CancellationTokenSource tempCancel = new CancellationTokenSource();
        var nextColor = RGBColor.Random();

        foreach (var group in effectGroups)
        {
          if (!skip)
          {
            group.SelectMany(x => x).SetState(cancellationToken, nextColor, 1);
          }
          else
          {
            group.SelectMany(x => x).SetBrightness(cancellationToken, 0);
            _ = group.FlashQuick(tempCancel.Token, null, IteratorEffectMode.AllIndividual, IteratorEffectMode.Random);
          }
          skip = !skip;
        }

        await Task.Delay(waitTime());
        tempCancel.Cancel();
        effectGroups = effectGroups.Skip(1).Union(effectGroups.Take(1)).ToList();
      }
    }
  }
}
