using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class AudioVolumeModifier : TimedCurveModifier<AudioSource, float>
	{
		#region Variables

		[SerializeField, MinMaxSlider(0f, 1f, true)]
		private Vector2 m_remap;

		#endregion

		#region Methods

		protected override float GetValue()
		{
			return modified.volume;
		}

		protected override float Lerp(float value)
		{
			return UnityUtil.Remap01(Mathf.Lerp(source, target, value), m_remap.x, m_remap.y);
		}

		protected override void SetValue(float value)
		{
			modified.volume = value;
		}

		#endregion
	}
}