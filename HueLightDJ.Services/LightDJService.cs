using HueApi.Entertainment.Extensions;
using HueLightDJ.Services;
using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;
using HueLightDJ.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Maui.Services
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

    public Task Connect(GroupConfiguration config)
    {
      //Connect
      effectService.StopEffects();
      return streamingSetup.SetupAndReturnGroupAsync(config);
    }

    public async Task<StatusModel> GetStatus()
    {
      StatusModel vm = new StatusModel();
      vm.bpm = StreamingSetup.GetBPM();
      vm.IsAutoMode = EffectService.IsAutoModeRunning();
      vm.AutoModeHasRandomEffects = EffectService.AutoModeHasRandomEffects;
      vm.ShowDisconnect = !(StreamingSetup.CurrentConnection?.HideDisconnect ?? false);
      vm.CurrentGroup = StreamingSetup.CurrentConnection?.Name;

      return vm;
    }

    public Task<EffectsVM> GetEffects()
    {
      return Task.FromResult(EffectService.GetEffectViewModels());
    }

    public void StartEffect(string typeName, string colorHex)
    {
      effectService.StartEffect(typeName, colorHex);
    }

    public void StartGroupEffect(string typeName, string colorHex, string groupName, string iteratorMode, string secondaryIteratorMode)
    {
      effectService.StartEffect(typeName, colorHex, groupName, Enum.Parse<IteratorEffectMode>(iteratorMode), Enum.Parse<IteratorEffectMode>(secondaryIteratorMode));
    }

    public Task IncreaseBPM(int value)
    {
      streamingSetup.IncreaseBPM(value);
      return GetStatus();

    }

    public Task SetBPM(int value)
    {
      streamingSetup.SetBPM(value);
      return GetStatus();

    }

    public void SetBri(double value)
    {
      streamingSetup.SetBrightnessFilter(value);
    }


    public void StartRandom()
    {
      effectService.StartRandomEffect();
    }

    public Task StartAutoMode()
    {
      effectService.StartAutoMode();
      return GetStatus();
    }

    public Task StopAutoMode()
    {
      effectService.StopAutoMode();
      return Task.CompletedTask;
    }

    public Task ToggleAutoRandomMode()
    {
      EffectService.AutoModeHasRandomEffects = !EffectService.AutoModeHasRandomEffects;
      return Task.CompletedTask;
    }

    public Task StopEffects()
    {
      effectService.StopAutoMode();
      effectService.StopEffects();
      return Task.CompletedTask;
    }

    public void SetColors(string[,] matrix)
    {
      ManualControlService.SetColors(matrix);
    }
    public void SetColorsList(List<List<string>> matrix)
    {
      ManualControlService.SetColors(matrix);
    }
    public void Beat(double intensity)
    {
      effectService.Beat(intensity);
    }

    public Task Disconnect()
    {
      effectService.CancelAllEffects();

      streamingSetup.Disconnect();
      return Task.CompletedTask;
    }

    
  }
}
