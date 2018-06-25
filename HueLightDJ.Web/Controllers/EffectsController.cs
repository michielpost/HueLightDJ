using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HueLightDJ.Effects;
using HueLightDJ.Effects.Base;
using HueLightDJ.Web.Models;
using HueLightDJ.Web.Streaming;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HueLightDJ.Web.Controllers
{
		  [Route("api/[controller]")]
		  [ApiController]
		  public class EffectsController : ControllerBase
		  {

					[Route("all")]
					public ActionResult<List<EffectViewModel>> GetEffects()
					{
							  return EffectService.GetEffectViewModels();
					}
		  }
}
