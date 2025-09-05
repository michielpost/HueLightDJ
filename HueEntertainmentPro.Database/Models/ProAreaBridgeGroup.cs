using System.ComponentModel.DataAnnotations.Schema;

namespace HueEntertainmentPro.Database.Models
{
  public class ProAreaBridgeGroup
  {
    public Guid Id { get; set; }

    public Guid ProAreaId { get; set; }

    [ForeignKey(nameof(ProAreaId))]
    public ProArea? ProArea { get; set; } = default!;


    public Guid BridgeId { get; set; }

    [ForeignKey(nameof(BridgeId))]
    public Bridge? Bridge { get; set; } = default!;


    /// <summary>
    /// Group Id on the bride
    /// </summary>
    public Guid GroupId { get; set; }

    public string? Name { get; set; }

  }
}
