using HueEntertainmentPro.Shared.Models;
using HueEntertainmentPro.Shared.Models.Requests;
using ProtoBuf.Grpc;
using System.ServiceModel;

namespace HueEntertainmentPro.Shared.Interfaces
{
  [ServiceContract]
  public interface IProAreaDataService
  {
    [OperationContract]
    Task<IEnumerable<ProArea>> GetProAreas(CallContext context = default);

    [OperationContract]
    Task<ProArea?> GetProArea(GuidRequest req, CallContext context = default);


    [OperationContract]
    Task DeleteProArea(GuidRequest req, CallContext context = default);

    [OperationContract]
    Task<ProArea> AddBridgeGroup(AddBridgeGroupRequest req, CallContext context = default);

    [OperationContract]
    Task DeleteBridgeGroup(GuidRequest req, CallContext context = default);
  }
}
