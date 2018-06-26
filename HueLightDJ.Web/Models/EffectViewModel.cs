using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Models
{
  public class EffectViewModel
  {
    public string Name { get; set; }

    public string TypeName { get; set; }
    public bool HasColorPicker { get; set; }


    //VueJS properties:
    public string Color { get; set; }

    public bool IsRandom { get; set; }

  }
}
