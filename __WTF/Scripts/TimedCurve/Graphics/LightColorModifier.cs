using UnityEngine;
using UnityEngine.Workshop;

namespace Groundling
{
	public class LightColorModifier : TimedCurveModifier<Light, Color>
	{
		#region Methods

		protected override Color GetValue()
		{
			return modified.color;
		}

		protected override Color Lerp(float value)
		{
			return Color.Lerp(source, target, value);
		}

		protected override void SetValue(Color value)
		{
			modified.color = value;
		}

		#endregion
	}
}
