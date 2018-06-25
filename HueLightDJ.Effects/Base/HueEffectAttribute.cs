using System;
using System.Collections.Generic;
using System.Text;

namespace HueLightDJ.Effects.Base
{
		  public class HueEffectAttribute : Attribute
		  {
					public string Name { get; set; }

					/// <summary>
					/// Runs on the Base Layer, cancels other base effects. Only one at a time
					/// </summary>
					public bool IsBaseEffect { get; set; } = true;

					public bool HasColorPicker { get; set; } = true;
		  }
}
