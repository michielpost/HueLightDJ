using HueLightDJ.Services.Interfaces.Models;
using HueLightDJ.Services.Interfaces.Models.Requests;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Interfaces
{
  [ServiceContract]
  public interface IHueSetupService
  {
    [OperationContract]
    Task<EntertainmentGroupResult> GetEntertainmentGroupsAsync(HueSetupRequest request, CallContext context = default);

    [OperationContract]
    Task IdentifyGroupsAsync(HueSetupRequest request, CallContext context = default);

    [OperationContract]
    Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CallContext context = default);

    [OperationContract]
    Task<RegisterEntertainmentResult?> RegisterAsync(HueSetupRequest request, CallContext context = default);
  }
}
