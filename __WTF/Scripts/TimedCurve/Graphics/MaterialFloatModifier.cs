using UnityEngine;

namespace Groundling
{
	public class MaterialFloatModifier : TimedCurveMaterialModifer<float>
	{
		#region Methods

		protected override float GetValue()
		{
			return material.GetFloat(propertyName);
		}

		protected override float Lerp(float value)
		{
			return Mathf.Lerp(source, target, value);
		}

		protected override void SetValue(float value)
		{
			material.SetFloat(propertyName, value);
		}

		#endregion
	}
}