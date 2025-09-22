using ProtoBuf;
using System;

namespace HueLightDJ.Services.Interfaces.Models
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class ConnectionConfiguration
  {
    public required string Ip { get; set; }
    public required string Key { get; set; }
    public required string EntertainmentKey { get; set; }
    public Guid? GroupId { get; set; }
    public bool UseSimulator { get; set; }
  }
}
