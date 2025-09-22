using System.Threading;

namespace HueLightDJ.Services.Models
{
  public class RunningEffectInfo
  {
    public CancellationTokenSource? CancellationTokenSource { get; set; }
    public required string Name { get; set; }
  }
}
