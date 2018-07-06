using HueLightDJ.Web.Models;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Streaming
{
  public static class GroupService
  {
    public static List<GroupModel> GetAll()
    {
      var layer = StreamingSetup.Layers.First();

      var allLightsOrdered = layer.OrderBy(x => x.LightLocation.X).ThenBy(x => x.LightLocation.Y).ToList();

      var leftRight = new[] { allLightsOrdered.GetLeft(), allLightsOrdered.GetRight() }.ToList();
      var frontBack = new[] { allLightsOrdered.GetFront(), allLightsOrdered.GetBack() }.ToList();
      var quarter = new[] { layer.GetLeft().GetFront(), layer.GetLeft().GetBack(), layer.GetRight().GetBack(), layer.GetRight().GetFront() }.ToList();
      var alternating = allLightsOrdered.ChunkByGroupNumber(2);
      var alternatingFour = allLightsOrdered.ChunkByGroupNumber(4);
      var orderedByDistance = layer.OrderBy(x => x.LightLocation.Distance(0, 0)).To2DGroup();
      var orderedByAngle = layer.OrderBy(x => x.LightLocation.Angle(0, 0)).To2DGroup();

      var result = new List<GroupModel>()
      {
        new GroupModel("All", allLightsOrdered.To2DGroup()),
        new GroupModel("Left/Right", leftRight),
        new GroupModel("Front/Back", frontBack),
        new GroupModel("Quarter", quarter),
        new GroupModel("Alternating", alternating),
        new GroupModel("Alternating by 4", alternatingFour),
        new GroupModel("Distance from center", orderedByDistance),
        new GroupModel("Order by Angle from center", orderedByAngle),
      };
     

      return result;
    }
  }
}
