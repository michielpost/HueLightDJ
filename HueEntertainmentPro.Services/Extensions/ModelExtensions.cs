using HueEntertainmentPro.Database.Models;

namespace HueEntertainmentPro.Services.Extensions
{
  public static class ModelExtensions
  {
    public static Shared.Models.Bridge ToApiModel(this Bridge input)
    {
      return new Shared.Models.Bridge
      {
        Id = input.Id,
        BridgeId = input.BridgeId,
        Ip = input.Ip,
        Name = input.Name,
        Username = input.Username,
        StreamingClientKey = input.StreamingClientKey
      };
    }

    public static Shared.Models.ProArea ToApiModel(this ProArea area)
    {
      return new HueEntertainmentPro.Shared.Models.ProArea
      {
        Id = area.Id,
        Name = area.Name,
        Connections = area.ProAreaBridgeGroups
           .Select(pg => new HueEntertainmentPro.Shared.Models.BridgeGroupConnection
           {
             Id = pg.Id,
             Bridge = pg.Bridge?.ToApiModel() ?? new(),
             GroupId = pg.GroupId,
             Name = pg.Name
           })
           .ToList()
      };
    }
  }
}
