using HueEntertainmentPro.Shared.Models;
using HueEntertainmentPro.Shared.Models.Requests;
using ProtoBuf.Grpc;
using System.ServiceModel;

namespace HueEntertainmentPro.Shared.Interfaces
{
  [ServiceContract]
  public interface IBridgeDataService
  {
    [OperationContract]
    Task<IEnumerable<Bridge>> GetBridges(CallContext context = default);

    [OperationContract]
    Task<Bridge?> GetBridge(GuidRequest req, CallContext context = default);

    [OperationContract]
    Task<Bridge?> AddBridge(AddBridgeRequest req, CallContext context = default);

    [OperationContract]
    Task DeleteBridge(GuidRequest req, CallContext context = default);
  }
}
