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

    public async Task Connect()
    {
      await Clients.All.SendAsync("StatusMsg", "Connecting to bridge...");

      try
      {
        //Connect
        await StreamingSetup.SetupAndReturnGroup();
        await Clients.All.SendAsync("StatusMsg", "Connected to bridge");

        var allEffects = EffectService.GetEffectViewModels();
        await Clients.All.SendAsync("effects", allEffects);

        await GetStatus();
      }
      catch
      {
        await Clients.All.SendAsync("StatusMsg", "Failed to connect to bridge");

      }
    }
    public void GetEffects()
    {
      var allEffects = EffectService.GetEffectViewModels();
      Clients.Caller.SendAsync("effects", allEffects);
    }

    public Task GetStatus()
    {
      StatusViewModel vm = new StatusViewModel();
      vm.bpm = StreamingSetup.GetBPM();

      //try
      //{
      //	//Connect
      //	bool isActive = await StreamingSetup.IsStreamingActive();
      //	if (isActive)
      //	{
      //			  await Clients.All.SendAsync("StatusMsg", "Streaming is active");

      //			  var allEffects = EffectService.GetEffectViewModels();
      //			  await Clients.Caller.SendAsync("effects", allEffects);
      //	}
      //	else
      //			  await Clients.All.SendAsync("StatusMsg", "Streaming is not active");
      //}
      //catch
      //{
      //	await Clients.All.SendAsync("StatusMsg", "Not connected to bridge.");

      //}

      return Clients.All.SendAsync("Status", vm);
    }

    public void StartEffect(string typeName, string colorHex)
    {
      EffectService.StartEffect(typeName, colorHex);
      Clients.All.SendAsync("StatusMsg", $"Started group effect {typeName} {colorHex}");

    }

    public void StartGroupEffect(string typeName, string colorHex, string groupName, string iteratorMode, string secondaryIteratorMode)
    {
      EffectService.StartEffect(typeName, colorHex, groupName, Enum.Parse<IteratorEffectMode>(iteratorMode), Enum.Parse<IteratorEffectMode>(secondaryIteratorMode));
      Clients.All.SendAsync("StatusMsg", $"Started group effect {typeName} {groupName} {colorHex}");

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

    public async Task Disconnect()
    {
      StreamingSetup.Disconnect();
      EffectService.CancelAllEffects();
      await Clients.Caller.SendAsync("effects", new EffectsVM());
      await Clients.All.SendAsync("StatusMsg", "DISCONNECTED...");
      GetStatus();
    }
  }
}
