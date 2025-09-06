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
        Username = input.Username
      };
    }
  }
}
