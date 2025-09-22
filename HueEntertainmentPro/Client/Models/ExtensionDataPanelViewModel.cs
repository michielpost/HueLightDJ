using HueApi.Models;

namespace HueEntertainmentPro.Client.Models
{
  public class ExtensionDataPanelViewModel
  {
    public HueResource HueResource { get; set; } = default!;
    public HueEntertainmentPro.Shared.Models.Bridge? Bridge { get; set; }
  }
}
