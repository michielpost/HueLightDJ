using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Models
{
  public class MultiBridgeHuePosition
  {
    public string Bridge { get; set; }
    public Guid GroupId { get; set; }

    public Guid Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
  }
}
