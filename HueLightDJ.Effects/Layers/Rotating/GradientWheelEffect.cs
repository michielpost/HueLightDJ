using HueLightDJ.Effects.Base;
using HueApi.ColorConverters;
using HueApi.ColorConverters.HSB;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Effects.BasEffects;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
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
    protected override int Steps => (int)(HSB.HueMaxValue * 0.1);
    //protected override bool DipToBlack => false;

  }
}
