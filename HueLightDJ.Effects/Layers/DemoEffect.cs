using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.Entertainment.Models;

namespace HueLightDJ.Effects.Layers
{
  /// <summary>
  /// Demo effect that shows some of the best effects
  /// </summary>
  [HueEffect(Name = "Demo Mode", Group = "Utils", HasColorPicker = false)]
  public class DemoEffect : IHueEffect
  {
    //Custom bpm for this effect
    private int _currentBpm = 75;
    public static Ref<TimeSpan> WaitTime { get; set; } = TimeSpan.FromMilliseconds(500);

    private CancellationTokenSource _cts = new CancellationTokenSource();

    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      cancellationToken.Register(() => {
        _cts.Cancel();
      });

      while (!cancellationToken.IsCancellationRequested)
      {
        SetBPM(75);

        _ = new HsbLoopEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        _ = new RandomColorRangeEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        SetBPM(20);

        _ = new RotatingEffect().Start(layer, () => WaitTime, color, _cts.Token);

        await Task.Delay(TimeSpan.FromSeconds(1));
        for (int i = 0; i < 25; i++)
        {
          SetBPM(20 + (i * 5));
          await Task.Delay(TimeSpan.FromMilliseconds(500));
        }
        await Task.Delay(TimeSpan.FromSeconds(2));

        ResetCts();

        SetBPM(75);
        _ = new RandomPulseEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(8), cancellationToken);
        ResetCts();

        _ = new GradientWheelEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        _ = new RandomPulseRetraceEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        _ = new RainbowWheelEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        _ = new RandomSingleRowBottomTopEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        _ = new RandomPulseTopBottomEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(8), cancellationToken);
        ResetCts();

        _ = new RainbowBottomTopEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(12), cancellationToken);
        ResetCts();

        _ = new QuickFlashFrontBackEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();
      }

    }

    private void ResetCts()
    {
      _cts.Cancel();
      _cts = new CancellationTokenSource();
    }

    private void SetBPM(int bpm)
    {
      _currentBpm = bpm;
      WaitTime.Value = TimeSpan.FromMilliseconds((60 * 1000) / bpm);
    }
  }
}
