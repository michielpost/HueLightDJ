using Q42.HueApi.Models.Groups;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Streaming
{
  public static class ManualControlService
  {
    public static void SetColors(string[,] matrix)
    {
      var lengthIndex = matrix.GetUpperBound(0);

      var lightsMatrix = new List<EntertainmentLight>[lengthIndex+1, lengthIndex+1];

      var allLights = StreamingSetup.Layers.First();
      foreach (var light in allLights)
      {
        int x = GetMatrixPositionX(light.LightLocation, lengthIndex+1);
        int y = GetMatrixPositionY(light.LightLocation, lengthIndex+1);

        if (lightsMatrix[x, y] == null)
          lightsMatrix[x, y] = new List<EntertainmentLight>();

        lightsMatrix[x, y].Add(light);
      }

      for (int x = 0; x <= lengthIndex; x++)
      {
        //var upper = matrix.GetUpperBound(x);
        for (int y = 0; y <= lengthIndex; y++)
        {
          if (!string.IsNullOrEmpty(matrix[x, y]))
          {
            if (lightsMatrix[x, y]?.Any() ?? false)
            {
              var color = matrix[x, y];
              foreach (var current in lightsMatrix[x, y])
              {
                current.State.SetRGBColor(new Q42.HueApi.ColorConverters.RGBColor(color));
                current.State.SetBrightness(1);
              }
            }
          }
        }
      }
    }

    private static int GetMatrixPositionX(LightLocation lightLocation, int matrixSize)
    {
      double pos = ((lightLocation.X +1) / 2) * matrixSize;
      return (int)pos;
    }
    private static int GetMatrixPositionY(LightLocation lightLocation, int matrixSize)
    {
      double pos = ((lightLocation.Y + 1) / 2) * matrixSize;
      return (int)pos;
    }
  }
}
