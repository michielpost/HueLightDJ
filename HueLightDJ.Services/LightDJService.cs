using HueApi.Entertainment.Extensions;
using HueLightDJ.Services;
using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;
using HueLightDJ.Services.Interfaces.Models.Requests;
using HueLightDJ.Services.Models;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Services
{
  public class LightDJService : ILightDJService
  {
    private readonly EffectService effectService;
    private readonly StreamingSetup streamingSetup;

    public LightDJService(EffectService effectService, StreamingSetup streamingSetup)
    {
      this.effectService = effectService;
      this.streamingSetup = streamingSetup;
    }

    public Task Connect(GroupConfiguration config, CallContext context = default)
    {
      //Connect
      effectService.StopEffects();
      return streamingSetup.SetupAndReturnGroupAsync(config);
    }

    public async Task<StatusModel> GetStatus(CallContext context = default)
    {

      StatusModel vm = new StatusModel();
      vm.bpm = StreamingSetup.GetBPM();
      vm.IsAutoMode = EffectService.IsAutoModeRunning();
      vm.AutoModeHasRandomEffects = EffectService.AutoModeHasRandomEffects;
      vm.ShowDisconnect = !(StreamingSetup.CurrentConnection?.HideDisconnect ?? false);
      vm.CurrentGroup = StreamingSetup.CurrentConnection?.Name;

      if (StreamingSetup.CurrentConnection != null)
      {
        var groups = GroupService.GetAll();
        vm.Groups = groups.Select(x => new GroupInfoViewModel() { Name = x.Name }).ToList();
      }

      return vm;
    }

    public Task<EffectsVM> GetEffects(CallContext context = default)
    {
      return Task.FromResult(EffectService.GetEffectViewModels());
    }

    public void StartEffect(StartEffectRequest request, CallContext context = default)
    {
      effectService.StartEffect(request.TypeName, request.ColorHex);
    }

    public void StartGroupEffect(StartEffectRequest request, CallContext context = default)
    {
      effectService.StartEffect(request.TypeName, request.ColorHex, request.GroupName, Enum.Parse<IteratorEffectMode>(request.IteratorMode!), Enum.Parse<IteratorEffectMode>(request.SecondaryIteratorMode!));
    }

    public Task IncreaseBPM(IntRequest req, CallContext context = default)
    {
      streamingSetup.IncreaseBPM(req.Value);
      return GetStatus();

    }

    public Task SetBPM(IntRequest req, CallContext context = default)
    {
      streamingSetup.SetBPM(req.Value);
      return GetStatus();

    }

    public void SetBri(DoubleRequest req, CallContext context = default)
    {
      streamingSetup.SetBrightnessFilter(req.Value);
    }


    public void StartRandom(CallContext context = default)
    {
      effectService.StartRandomEffect();
    }

    public Task StartAutoMode(CallContext context = default)
    {
      effectService.StartAutoMode();
      return GetStatus();
    }

    public Task StopAutoMode(CallContext context = default)
    {
      effectService.StopAutoMode();
      return Task.CompletedTask;
    }

    public Task SetAutoRandomMode(BoolRequest req, CallContext context = default)
    {
      EffectService.AutoModeHasRandomEffects = req.Value;
      return Task.CompletedTask;
    }

    [Obsolete]
    public Task ToggleAutoRandomMode(CallContext context = default)
    {
      EffectService.AutoModeHasRandomEffects = !EffectService.AutoModeHasRandomEffects;
      return Task.CompletedTask;
    }

    public Task StopEffects(CallContext context = default)
    {
      effectService.StopAutoMode();
      effectService.StopEffects();
      return Task.CompletedTask;
    }

    //public void SetColors(string[,] matrix)
    //{
    //  ManualControlService.SetColors(matrix);
    //}
    //public void SetColorsList(List<List<string>> matrix)
    //{
    //  ManualControlService.SetColors(matrix);
    //}
    public void Beat(DoubleRequest req, CallContext context = default)
    {
      effectService.Beat(req.Value);
    }

    public Task Disconnect(CallContext context = default)
    {
      effectService.CancelAllEffects();

      streamingSetup.Disconnect();
      return Task.CompletedTask;
    }

    
  }
}
