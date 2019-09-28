using UnityEngine;
using UnityEngine.Workshop;

namespace Groundling
{
	public class LightIntensityModifier : TimedCurveModifier<Light, float>
	{
		#region Methods

		protected override float GetValue()
		{
			return modified.intensity;
		}

		protected override float Lerp(float value)
		{
			return Mathf.Lerp(source, target, value);
		}

		protected override void SetValue(float value)
		{
			modified.intensity = value;
		}

		#endregion
	}
}