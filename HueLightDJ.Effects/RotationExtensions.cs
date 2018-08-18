using System;
using System.Collections.Generic;
using System.Text;

namespace HueLightDJ.Effects
{
  public static class RotationExtensions
  {
    public static double Move360(this double input, double moveBy)
    {
      var newValue = input + moveBy;
      if (newValue < 0)
        return newValue + 360;
      if (newValue > 360)
        return newValue - 360;

      return newValue;

    }
  }
}
