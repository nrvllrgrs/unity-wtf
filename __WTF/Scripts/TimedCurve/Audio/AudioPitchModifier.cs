using UnityEngine;
using UnityEngine.Workshop;

namespace Groundling
{
	public class AudioPitchModifier : TimedCurveModifier<AudioSource, float>
	{
		#region Methods

		protected override float GetValue()
		{
			return modified.pitch;
		}

		protected override float Lerp(float value)
		{
			return Mathf.Lerp(source, target, value);
		}

		protected override void SetValue(float value)
		{
			modified.pitch = value;
		}

		#endregion
	}
}