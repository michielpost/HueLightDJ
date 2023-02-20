using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Models
{
  public class MultiBridgeHuePosition
  {
    public required string Bridge { get; set; }
    public Guid GroupId { get; set; }

    public Guid Id { get; set; }

    public int PositionIndex { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
  }
}
