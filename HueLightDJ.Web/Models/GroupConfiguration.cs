using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Q42.HueApi.Models.Groups;

namespace HueLightDJ.Web.Models
{
  public class GroupConfiguration
  {
    public string Name { get; set; }
    public List<ConnectionConfiguration> Connections { get; set; }
    public LightLocation LocationCenter { get; set; }
    public bool IsAlwaysVisible { get; set; }
  }
}
