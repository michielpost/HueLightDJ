using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Models
{
    public class PreviewModel
    {
    public string Bridge { get; set; }
    public byte Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public string Hex { get; set; }
    public double Bri { get; set; }
  }
}
