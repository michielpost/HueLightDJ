using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Models
{
  public class EffectsVM
  {
    public List<EffectViewModel> BaseEffects { get; set; }
    public List<EffectViewModel> ShortEffects { get; set; }
    public List<EffectViewModel> GroupEffects { get; set; }
    public List<GroupInfoViewModel> Groups { get; set; }
    public List<string> IteratorModes { get; set; }
    public List<string> SecondaryIteratorModes { get; set; }

  }

  public class EffectViewModel
  {
    public string Name { get; set; }

    public string TypeName { get; set; }
    public bool HasColorPicker { get; set; }


    //VueJS properties:
    public string Color { get; set; }

    public bool IsRandom { get; set; } = true;

  }

  public class GroupInfoViewModel
  {
    public string Name { get; set; }

  }
}
