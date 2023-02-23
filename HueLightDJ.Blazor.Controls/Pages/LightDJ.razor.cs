using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls.Pages
{
  public partial class LightDJ
  {
    [Parameter]
    public Guid Id { get; set; }

    GroupConfiguration? config;


    protected override async Task OnParametersSetAsync()
    {
      config = await LocalStorageService.Get(Id);
      await base.OnParametersSetAsync();
    }

    public Task Connect()
    {
      return LightDJService.Connect(config);
    }

    public Task Disconnect()
    {
      return LightDJService.Disconnect();
    }

  }
}
