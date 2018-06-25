using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Models
{
    public class RunningEffectInfo
    {
					public CancellationTokenSource CancellationTokenSource { get; set; }
					public string Name { get; set; }
		  }
}
