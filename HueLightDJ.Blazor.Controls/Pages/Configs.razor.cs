using HueLightDJ.Services.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls.Pages
{
  public partial class Configs
  {
    List<GroupConfiguration> configs = new List<GroupConfiguration>();

    public async Task AddNew()
    {
      var config = await LocalStorageService.Add("HueLightDJ Group");
      NavManager.NavigateTo($"/config-edit/{config.Id}");
    }

    protected override async Task OnInitializedAsync()
    {
      configs = await LocalStorageService.GetAllAsync();

      await base.OnInitializedAsync();
    }
  }
}
