namespace HueLightDJ.Services.Models
{
  public class EffectLogMsg
  {
    public required string Name { get; set; }

    public string? EffectType { get; set; }
    public string? RGBColor { get; set; }
    public string? Group { get; set; }
    public string? IteratorMode { get; set; }
    public string? SecondaryIteratorMode { get; set; }
  }
}
