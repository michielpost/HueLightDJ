using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Models
{
  public class GroupConfiguration
  {
    public string Name { get; set; }
    public List<ConnectionConfiguration> Connections { get; set; }
  }
}
