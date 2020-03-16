using TMPro;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop.UI
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class HealthTextMeshUGUI : MonoBehaviour
	{
		#region Variables

		[SerializeField, Required]
		private Health m_health;

		[SerializeField]
		private string m_format = "{0}/<size=55%>{1}</size>";

		[SerializeField]
		private string m_numericFormat = "#,##0";

		private TextMeshProUGUI m_textMesh;

		#endregion

		#region Properties

		public TextMeshProUGUI textMesh => this.GetComponent(ref m_textMesh);

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_health.ValueChanged += ValueChanged;
		}

		private void OnDisable()
		{
			m_health.ValueChanged -= ValueChanged;
		}

		private void ValueChanged(object sender, System.EventArgs e)
		{
			textMesh.text = string.Format(m_format, Mathf.CeilToInt(m_health.value).ToString(m_numericFormat), m_health.maxHealth.ToString(m_numericFormat));
		}

		private void OnValidate()
		{
			try
			{
				ValueChanged(this, System.EventArgs.Empty);
			}
			catch { }
		}

		#endregion
	}
}