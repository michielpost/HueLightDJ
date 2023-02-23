using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls.Pages
{
  public partial class LightDJ : IDisposable
  {
    [Parameter]
    public Guid Id { get; set; }

    GroupConfiguration? config;

    private string lastMsg = string.Empty;
    private StatusModel statusModel = new();
    private EffectsVM effectsVM = new();
    private int Brightness = 100;


    protected override async Task OnParametersSetAsync()
    {
      config = await LocalStorageService.Get(Id);
      effectsVM = await LightDJService.GetEffects();

      await base.OnParametersSetAsync();
    }

    protected override void OnInitialized()
    {
      HubService.LogMsgEvent += HubService_LogMsgEvent;
      HubService.StatusChangedEvent += HubService_StatusChangedEvent;
      base.OnInitialized();
    }

    private async void HubService_StatusChangedEvent(object? sender, EventArgs e)
    {
      await GetStatusAsync();
    }

    private void HubService_LogMsgEvent(object? sender, string? e)
    {
      lastMsg = e ?? string.Empty;
    }

    private async Task GetStatusAsync()
    {
      statusModel = await LightDJService.GetStatus();
    }

    public async Task Connect()
    {
      await LightDJService.Connect(config);
    }

    public async Task Disconnect()
    {
      await LightDJService.Disconnect();
    }

    public void Dispose()
    {
      HubService.LogMsgEvent -= HubService_LogMsgEvent;
      HubService.StatusChangedEvent -= HubService_StatusChangedEvent;
    }
  }
}
