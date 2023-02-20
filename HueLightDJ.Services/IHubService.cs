using HueLightDJ.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Services
{
  public interface IHubService
  {
    Task SendAsync(string method, object? arg1);
    Task SendAsync(string method, object? arg1, object? arg2);
  }
}
