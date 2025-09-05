using HueEntertainmentPro.Database;
using HueEntertainmentPro.Database.Models;
using HueEntertainmentPro.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace HueEntertainmentPro.Services
{
  public class BridgeDataService(HueEntertainmentProDbContext dbContext) : IBridgeDataService
  {
    public async Task<Shared.Models.Bridge> AddBridge(Shared.Models.Requests.AddBridgeRequest req, CallContext context = default)
    {
      var bridge = await dbContext.Bridges.Where(x => x.Ip == req.Ip).FirstOrDefaultAsync();
      if (bridge == null)
      {
        bridge = new Bridge()
        {
          Id = Guid.NewGuid(),
          Ip = req.Ip,
          Username = req.Username,
          StreamingClientKey = req.StreamingClientKey,
          Name = req.Name
        };
        dbContext.Bridges.Add(bridge);
      }
      else
      {
        bridge.Ip = req.Ip;
        bridge.Username = req.Username;
        bridge.StreamingClientKey = req.StreamingClientKey;
        bridge.Name = req.Name;
      }

      await dbContext.SaveChangesAsync();

     return new Shared.Models.Bridge()
      {
        Id = bridge.Id,
        Ip = bridge.Ip,
        Name = bridge.Name
      };

    }

    public async Task DeleteBridge(Shared.Models.Requests.GuidRequest req, CallContext context = default)
    {
      var bridge = await dbContext.Bridges.Where(x => x.Id == req.Id).FirstOrDefaultAsync();
      if(bridge != null)
      {
        dbContext.Bridges.Remove(bridge);
        await dbContext.SaveChangesAsync();
      }
    }

    public async Task<Shared.Models.Bridge?> GetBridge(Shared.Models.Requests.GuidRequest req, CallContext context = default)
    {
      var bridge = await dbContext.Bridges.Where(x => x.Id == req.Id).FirstOrDefaultAsync();
      if(bridge == null)
        return null;

      return new Shared.Models.Bridge()
      {
        Id = bridge.Id,
        Ip = bridge.Ip,
        Name = bridge.Name
      };
    }

    public async Task<IEnumerable<Shared.Models.Bridge>> GetBridges(CallContext context = default)
    {
      var all = await dbContext.Bridges.ToListAsync();
      return all.Select(bridge => new Shared.Models.Bridge()
      {
        Id = bridge.Id,
        Ip = bridge.Ip,
        Name = bridge.Name
      });
    }
  }
}
