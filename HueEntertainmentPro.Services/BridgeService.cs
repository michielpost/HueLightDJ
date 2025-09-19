using HueEntertainmentPro.Database;
using HueEntertainmentPro.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace HueEntertainmentPro.Services
{
  public class BridgeService(HueEntertainmentProDbContext dbContext)
  {

    public async Task<Bridge?> GetBridge(Guid id)
    {
      var bridge = await dbContext.Bridges.Where(x => x.Id == id).FirstOrDefaultAsync();
      if (bridge == null)
        return null;

      return bridge;
    }

  }
}
