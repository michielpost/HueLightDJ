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
    public static void SetBPM(int bpm)
    {
      BPM = bpm;
      WaitTime.Value = TimeSpan.FromMilliseconds((60 * 1000) / bpm);
    }

    public static void SetColors(string[,] matrix)
    {
      var heightIndex = matrix.GetUpperBound(0);
      var widthIndex = matrix.GetUpperBound(1);

      var lightsMatrix = new List<EntertainmentLight>[heightIndex + 1, widthIndex + 1];

      var allLights = StreamingSetup.Layers.First();
      foreach (var light in allLights)
      {
        int x = GetMatrixPositionY(light.LightLocation, heightIndex + 1);
        int y = GetMatrixPositionX(light.LightLocation, widthIndex + 1);

        if (lightsMatrix[x, y] == null)
          lightsMatrix[x, y] = new List<EntertainmentLight>();

        lightsMatrix[x, y].Add(light);
      }

      for (int x = 0; x <= heightIndex; x++)
      {
        //var upper = matrix.GetUpperBound(x);
        for (int y = 0; y <= widthIndex; y++)
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

    public static void SetColors(List<List<string>> matrix)
    {
      int height = matrix.Count();
      int maxWidth = matrix.Max(x => x.Count);

      var array = new string[height, maxWidth];

      for (int x = 0; x < height; x++)
      {
        if (matrix[x]?.Any() ?? false)
        {
          for (int y = 0; y < matrix[x].Count; y++)
          {
            array[x, y] = matrix[x][y];
          }
        }
      }

      SetColors(array);
    }

    private static int GetMatrixPositionX(LightLocation lightLocation, int matrixSize)
    {
      double pos = ((lightLocation.X +1) / 2) * matrixSize;
      return (int)pos;
    }
    private static int GetMatrixPositionY(LightLocation lightLocation, int matrixSize)
    {
      double pos = ((1 - (lightLocation.Y + 1) / 2)) * matrixSize;
      return (int)pos;
    }
  }
}
