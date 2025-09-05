namespace HueEntertainmentPro.Database.Models
{
  public class ProArea
  {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    //public bool IsAlwaysVisible { get; set; }
    //public bool HideDisconnect { get; set; }

    public IList<ProAreaBridgeGroup> ProAreaBridgeGroups { get; set; }
  }
}
