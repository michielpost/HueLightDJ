using HueLightDJ.Services.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Interfaces
{
  public interface IHubService
  {
    event EventHandler<string?>? LogMsgEvent;

    event EventHandler? StatusChangedEvent;

    event EventHandler<IEnumerable<PreviewModel>>? PreviewEvent;


    Task StatusChanged();

    Task SendAsync(string method, params object?[] arg1);
    Task SendPreview(IEnumerable<PreviewModel> list);
  }
}
