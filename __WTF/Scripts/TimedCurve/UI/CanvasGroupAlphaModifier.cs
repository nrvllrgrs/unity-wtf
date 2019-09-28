using UnityEngine;
using UnityEngine.Workshop;

namespace Groundling
{
	public class CanvasGroupAlphaModifier : TimedCurveModifier<CanvasGroup, float>
	{
		protected override float GetValue()
		{
			return modified.alpha;
		}

		protected override float Lerp(float value)
		{
			return Mathf.Lerp(source, target, value);
		}

		protected override void SetValue(float value)
		{
			modified.alpha = value;
		}
	}
}