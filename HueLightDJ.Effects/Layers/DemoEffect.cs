using HueApi.ColorConverters;
using HueApi.Entertainment.Models;
using HueLightDJ.Effects.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

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
      cancellationToken.Register(() =>
      {
        _cts.Cancel();
      });

      while (!cancellationToken.IsCancellationRequested)
      {
        SetBPM(75);

        new HsbLoopEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        new RandomColorRangeEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        SetBPM(20);

        new RotatingEffect().Start(layer, () => WaitTime, color, _cts.Token);

        await Task.Delay(TimeSpan.FromSeconds(1));
        for (int i = 0; i < 25; i++)
        {
          SetBPM(20 + (i * 5));
          await Task.Delay(TimeSpan.FromMilliseconds(500));
        }
        await Task.Delay(TimeSpan.FromSeconds(2));

        ResetCts();

        SetBPM(75);
        new RandomPulseEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(8), cancellationToken);
        ResetCts();

        new GradientWheelEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        new RandomPulseRetraceEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        new RainbowWheelEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        new RandomSingleRowBottomTopEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
        ResetCts();

        new RandomPulseTopBottomEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(8), cancellationToken);
        ResetCts();

        new RainbowBottomTopEffect().Start(layer, () => WaitTime, color, _cts.Token);
        await Task.Delay(TimeSpan.FromSeconds(12), cancellationToken);
        ResetCts();

        new QuickFlashFrontBackEffect().Start(layer, () => WaitTime, color, _cts.Token);
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
