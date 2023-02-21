using HueApi.BridgeLocator;
using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls.Pages
{
  public partial class BridgeAdd
  {
    [Parameter]
    public Guid Id { get; set; }

    GroupConfiguration? config;
    IEnumerable<LocatedBridge> bridges = new List<LocatedBridge>();

    protected override async Task OnParametersSetAsync()
    {
      config = await LocalStorageService.Get(Id);
      bridges = await HueSetupService.LocateBridgesAsync();
      await base.OnParametersSetAsync();
    }

    public async Task Save()
    {
      //var config = await LocalStorageService.Add("HueLightDJ setup");
      //NavManager.NavigateTo($"/config-edit?id={config.Id}");
    }
  }
}
