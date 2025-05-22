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
using HueLightDJ.Services.Interfaces.Models;
using Microsoft.Extensions.Options;

namespace HueLightDJ.Web.Hubs
{
  public class StatusHub : Hub
  {
    private readonly EffectService effectService;
    private readonly StreamingSetup streamingSetup;
    private readonly List<GroupConfiguration> _groupConfigurations;

    public StatusHub(EffectService effectService, StreamingSetup streamingSetup, IOptions<List<GroupConfiguration>> configOptions)
    {
      this.effectService = effectService;
      this.streamingSetup = streamingSetup;
      this._groupConfigurations = configOptions.Value;
    }

    public async Task Connect(string groupName)
    {
      await Clients.All.SendAsync("StatusMsg", $"Connecting to bridge for group {groupName}...");

      try
      {
        var groupConfig = _groupConfigurations.FirstOrDefault(gc => gc.Name == groupName);
        if (groupConfig == null)
        {
            await Clients.All.SendAsync("StatusMsg", $"Failed to connect: Group {groupName} not found.");
            // Potentially throw or return early
            return;
        }
        //Connect
        await streamingSetup.SetupAndReturnGroupAsync(groupConfig);
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
      var configs = _groupConfigurations;
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
      effectService.StopAutoMode();
      return GetStatus();
    }

    public Task ToggleAutoRandomMode()
    {
      EffectService.AutoModeHasRandomEffects = !EffectService.AutoModeHasRandomEffects;
      return GetStatus();
    }

    public Task StopEffects()
    {
      effectService.StopAutoMode();
      effectService.StopEffects();
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
      effectService.CancelAllEffects();

      await streamingSetup.DisconnectAsync();
      await Clients.All.SendAsync("effects", new EffectsVM());
      await Clients.All.SendAsync("StatusMsg", "DISCONNECTED...");
      await GetStatus();
    }
  }
}
