using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop.UI
{
	[RequireComponent(typeof(Slider))]
	public class HealthSlider : MonoBehaviour
	{
		#region Variables

		[SerializeField, Required]
		private Health m_health;

		private Slider m_slider;

		#endregion

		#region Properties

		public Slider slider
		{
			get
			{
				if (m_slider == null)
				{
					m_slider = GetComponent<Slider>();
				}
				return m_slider;
			}
		}

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_health.ValueChanged += ValueChanged;
			ValueChanged(m_health, null);
		}

		private void OnDisable()
		{
			m_health.ValueChanged -= ValueChanged;
		}

		private void ValueChanged(object sender, System.EventArgs e)
		{
			var health = sender as Health;
			if (health != null)
			{
				slider.value = UnityUtil.Remap(
					health.value,
					0f,
					health.maxValue,
					slider.minValue,
					slider.maxValue);
			}
		}

		#endregion
	}
}
