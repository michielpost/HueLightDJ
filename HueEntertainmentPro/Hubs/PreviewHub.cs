using Microsoft.AspNetCore.SignalR;

namespace HueEntertainmentPro.Hubs
{
  public class PreviewHub : Hub
  {

    public PreviewHub()
    {
    }

    public async Task Connect()
    {
      await Clients.All.SendAsync("StatusMsg", $"Connected!");
    }

  }
}
