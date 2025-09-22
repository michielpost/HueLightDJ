using ProtoBuf;

namespace HueLightDJ.Services.Interfaces.Models
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class RegisterEntertainmentResult
  {
    public string? Ip { get; set; }

    public string? Username { get; set; }

    public string? StreamingClientKey { get; set; }

    public string? ErrorMessage { get; set; }
  }
}
