using HueLightDJ.Blazor.Controls.Services;
using HueLightDJ.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls.Shared
{
  public partial class Preview : IDisposable
  {
    [Inject]
    public HueJsInterop HueJsInterop { get; set; } = default!;

    [Inject]
    public IHubService HubService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
      await HueJsInterop.InitializePreview();
    }

    protected override void OnInitialized()
    {
      HubService.PreviewEvent += HubService_PreviewEvent;
      base.OnInitialized();
    }

    private void HubService_PreviewEvent(object? sender, IEnumerable<HueLightDJ.Services.Interfaces.Models.PreviewModel> e)
    {
      HueJsInterop.ShowPreview(e);
    }

    public void Dispose()
    {
      HubService.PreviewEvent -= HubService_PreviewEvent;
    }
  }

}
