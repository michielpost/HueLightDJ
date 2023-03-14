using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HueLightDJ.Services.Interfaces.Models;
using System.ServiceModel;
using HueLightDJ.Services.Interfaces.Models.Requests;
using ProtoBuf.Grpc;

namespace HueLightDJ.Services.Interfaces
{
  [ServiceContract]
  public interface ILightDJService
  {
    [OperationContract]
    Task Connect(GroupConfiguration config, CallContext context = default);

    [OperationContract]
    Task<StatusModel> GetStatus(CallContext context = default);

    [OperationContract]
    Task<EffectsVM> GetEffects(CallContext context = default);

    [OperationContract]
    void StartEffect(StartEffectRequest request, CallContext context = default);

    [OperationContract]
    void StartGroupEffect(StartEffectRequest request, CallContext context = default);

    [OperationContract]
    Task IncreaseBPM(IntRequest value, CallContext context = default);

    [OperationContract]
    Task SetBPM(IntRequest value, CallContext context = default);

    [OperationContract]
    void SetBri(DoubleRequest value, CallContext context = default);

    [OperationContract]
    void StartRandom(CallContext context = default);

    [OperationContract]
    Task StartAutoMode(CallContext context = default);

    [OperationContract]
    Task StopAutoMode(CallContext context = default);

    [OperationContract]
    Task SetAutoRandomMode(BoolRequest value, CallContext context = default);

    [OperationContract]
    Task StopEffects(CallContext context = default);

    //void SetColors(string[,] matrix);

    //void SetColorsList(List<List<string>> matrix);

    [OperationContract]
    void Beat(DoubleRequest value, CallContext context = default);

    [OperationContract]
    Task Disconnect(CallContext context = default);
  }
}
