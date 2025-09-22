using HueApi.ColorConverters.HSB;
using HueLightDJ.Effects.Base;

namespace HueLightDJ.Effects.Layers
{
  [HueEffect(Order = 3, Name = "Gradient Wheel Effect", Group = "Rotating", HasColorPicker = false)]
  public class GradientWheelEffect : RainbowWheelEffect, IHueEffect
  {
    protected override int Steps => (int)(HSB.HueMaxValue * 0.1);
    //protected override bool DipToBlack => false;

  }
}
