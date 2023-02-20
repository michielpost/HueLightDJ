using Microsoft.AspNetCore.SignalR;
using HueApi.Entertainment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLightDJ.Services.Models;
using HueLightDJ.Services;
using HueLightDJ.Web.Models;
using Microsoft.Extensions.Configuration;

namespace HueLightDJ.Web.Hubs
{
  public class StatusHub : Hub
  {
    private readonly EffectService effectService;
    private readonly StreamingSetup streamingSetup;

    public StatusHub(EffectService effectService, StreamingSetup streamingSetup)
    {
      this.effectService = effectService;
      this.streamingSetup = streamingSetup;
    }

    public async Task Connect(string groupName)
    {
      await Clients.All.SendAsync("StatusMsg", $"Connecting to bridge for group {groupName}...");

      try
      {
        //Connect
        await streamingSetup.SetupAndReturnGroupAsync(groupName);
        await Clients.All.SendAsync("StatusMsg", "Connected to bridge");

        await GetEffects(true);
        await GetStatus();
      }
      catch(Exception ex)
      {
        await Clients.All.SendAsync("StatusMsg", $"Failed to connect to bridge for group {groupName}, " + ex);

      }
    }

    public async Task GetStatus()
    {
      var configs = await streamingSetup.GetGroupConfigurationsAsync();
      StatusViewModel vm = new StatusViewModel();
      vm.bpm = StreamingSetup.GetBPM();
      vm.IsAutoMode = EffectService.IsAutoModeRunning();
      vm.AutoModeHasRandomEffects = EffectService.AutoModeHasRandomEffects;
      vm.ShowDisconnect = !(StreamingSetup.CurrentConnection?.HideDisconnect ?? false);
      vm.GroupNames = configs.Select(x => x.Name).ToList();
      vm.CurrentGroup = StreamingSetup.CurrentConnection?.Name;

      await Clients.All.SendAsync("Status", vm);
    }

    public Task GetEffects(bool forAll)
    {
      if (StreamingSetup.Layers?.Count > 0)
      {
        var allEffects = EffectService.GetEffectViewModels();

        if(forAll)
          return Clients.All.SendAsync("effects", allEffects);
        else
          return Clients.Caller.SendAsync("effects", allEffects);

      }

      return Task.CompletedTask;
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
      StreamingSetup.IncreaseBPM(value);
      return GetStatus();

    }

    public Task SetBPM(int value)
    {
      StreamingSetup.SetBPM(value);
      return GetStatus();

    }

    public void SetBri(double value)
    {
      StreamingSetup.SetBrightnessFilter(value);

      Clients.Others.SendAsync("Bri", value);
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
      EffectService.StopAutoMode();
      return GetStatus();
    }

    public Task ToggleAutoRandomMode()
    {
      EffectService.AutoModeHasRandomEffects = !EffectService.AutoModeHasRandomEffects;
      return GetStatus();
    }

    public Task StopEffects()
    {
      EffectService.StopAutoMode();
      EffectService.StopEffects();
      return GetStatus();
    }

    public Task SendStatus(string msg)
    {
      return Clients.All.SendAsync("StatusMsg", msg);
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

    public async Task Disconnect()
    {
      EffectService.CancelAllEffects();

      StreamingSetup.Disconnect();
      await Clients.All.SendAsync("effects", new EffectsVM());
      await Clients.All.SendAsync("StatusMsg", "DISCONNECTED...");
      await GetStatus();
    }
  }
}
