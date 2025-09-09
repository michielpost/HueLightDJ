namespace HueEntertainmentPro.Database.Models
{
  public class Bridge
  {
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public required string Ip { get; set; }
    public required string BridgeId { get; set; }

    public required string Username { get; set; }

    public required string StreamingClientKey { get; set; }

    public DateTime CreatedDate { get; set; }
  }
}
