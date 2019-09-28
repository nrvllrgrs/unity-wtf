using UnityEngine;

namespace Groundling
{
	public class MaterialColorModifier : TimedCurveMaterialModifer<Color>
	{
		#region Methods

		protected override Color GetValue()
		{
			return material.GetColor(propertyName);
		}

		protected override Color Lerp(float value)
		{
			return Color.Lerp(source, target, value);
		}

		protected override void SetValue(Color value)
		{
			material.SetColor(propertyName, value);
		}

		#endregion
	}
}