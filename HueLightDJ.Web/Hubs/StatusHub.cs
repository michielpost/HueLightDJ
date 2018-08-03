using HueLightDJ.Web.Models;
using HueLightDJ.Web.Streaming;
using Microsoft.AspNetCore.SignalR;
using Q42.HueApi.Streaming.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Hubs
{
  public class StatusHub : Hub
  {

    public async Task Connect(bool demoMode = false)
    {
      await Clients.All.SendAsync("StatusMsg", "Connecting to bridge...");
      if(demoMode)
        await Clients.All.SendAsync("StatusMsg", "Starting DEMO mode");

      try
      {
        //Connect
        await StreamingSetup.SetupAndReturnGroup(demoMode);
        await Clients.All.SendAsync("StatusMsg", "Connected to bridge");

        await GetStatus();
        GetEffects(true);
      }
      catch(Exception ex)
      {
        await Clients.All.SendAsync("StatusMsg", "Failed to connect to bridge, " + ex);

      }
    }

    public Task GetStatus()
    {
      StatusViewModel vm = new StatusViewModel();
      vm.bpm = StreamingSetup.GetBPM();
      vm.IsAutoMode = EffectService.IsAutoModeRunning();

      return Clients.All.SendAsync("Status", vm);
    }

    public void GetEffects(bool forAll)
    {
      if (StreamingSetup.Layers?.Count > 0)
      {
        var allEffects = EffectService.GetEffectViewModels();

        if(forAll)
          Clients.All.SendAsync("effects", allEffects);
        else
          Clients.Caller.SendAsync("effects", allEffects);

      }
    }

    public void StartEffect(string typeName, string colorHex)
    {
      EffectService.StartEffect(typeName, colorHex);
    }

    public void StartGroupEffect(string typeName, string colorHex, string groupName, string iteratorMode, string secondaryIteratorMode)
    {
      EffectService.StartEffect(typeName, colorHex, groupName, Enum.Parse<IteratorEffectMode>(iteratorMode), Enum.Parse<IteratorEffectMode>(secondaryIteratorMode));
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
      EffectService.StartRandomEffect();
    }

    public void StartAutoMode()
    {
      EffectService.StartAutoMode();
      GetStatus();
    }

    public void StopAutoMode()
    {
      EffectService.StopAutoMode();
      GetStatus();
    }

    public Task SendStatus(string msg)
    {
      return Clients.All.SendAsync("StatusMsg", msg);
    }

    public async Task Disconnect()
    {
      EffectService.CancelAllEffects();

      StreamingSetup.Disconnect();
      await Clients.All.SendAsync("effects", new EffectsVM());
      await Clients.All.SendAsync("StatusMsg", "DISCONNECTED...");
      GetStatus();
    }
  }
}
