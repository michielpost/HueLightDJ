using HueLightDJ.Services.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls.Pages
{
  public partial class Index
  {
    List<GroupConfiguration> configs = new List<GroupConfiguration>();

    public async Task AddNew()
    {
      var config = await LocalStorageService.Add("HueLightDJ Group");
      NavManager.NavigateTo($"/config-edit/{config.Id}");
    }

    public async Task Remove(Guid id)
    {
      await LocalStorageService.Delete(id);
      configs = await LocalStorageService.GetAllAsync();
    }

    protected override async Task OnInitializedAsync()
    {
      configs = await LocalStorageService.GetAllAsync();

      await base.OnInitializedAsync();
    }
  }
}
