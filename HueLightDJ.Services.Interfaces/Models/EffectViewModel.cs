using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Interfaces.Models
{
  public class EffectsVM
  {
    public Dictionary<string, List<EffectViewModel>> BaseEffects { get; set; } = new();
    public List<EffectViewModel> ShortEffects { get; set; } = new();
    public List<EffectViewModel> GroupEffects { get; set; } = new();
    public List<GroupInfoViewModel> Groups { get; set; } = new();
    public List<string> IteratorModes { get; set; } = new();
    public List<string> SecondaryIteratorModes { get; set; } = new();

  }

  public class EffectViewModel
  {
    public required string Name { get; set; }

    public required string TypeName { get; set; }
    public bool HasColorPicker { get; set; }


    //VueJS properties:
    public string? Color { get; set; }

    public bool IsRandom { get; set; } = true;

  }

  public class GroupInfoViewModel
  {
    public required string Name { get; set; }

  }
}
