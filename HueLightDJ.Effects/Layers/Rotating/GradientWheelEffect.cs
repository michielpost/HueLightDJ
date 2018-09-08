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
  [HueEffect(Order = 3, Name = "Gradient Wheel Effect", Group = "Rotating", HasColorPicker = false)]
  public class GradientWheelEffect : RainbowWheelEffect, IHueEffect
  {
    protected override int Steps => (int)(65535 * 0.1);
    //protected override bool DipToBlack => false;

  }
}
