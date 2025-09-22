using System;

namespace HueLightDJ.Effects.Base
{
  public class HueEffectAttribute : Attribute
  {

    public int Order { get; set; } = 100;

    public required string Name { get; set; }
    public string Group { get; set; } = "Other";

    /// <summary>
    /// Runs on the Base Layer, cancels other base effects. Only one at a time
    /// </summary>
    public bool IsBaseEffect { get; set; } = true;

    public bool HasColorPicker { get; set; } = true;

    public string? DefaultColor { get; set; }
  }
}
