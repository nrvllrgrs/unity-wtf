using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public abstract class TimedCurveModifier<K, T> : MonoBehaviour
		where K : Component
	{
		#region Variables

		[Required, PropertyOrder(-10)]
		public TimedCurve timedCurve;

		[SerializeField, BoxGroup("Modifier Settings"), ShowIf("ShowModified"), PropertyOrder(-1)]
		private K m_modified;

		[SerializeField, LabelText("Override Source"), BoxGroup("Modifier Settings")]
		private bool m_overrideSourceValue = false;

		[SerializeField, ShowIf("m_overrideSourceValue"), BoxGroup("Modifier Settings")]
		private bool m_overrideOnStart = true;

		[EnableIf("m_overrideSourceValue"), BoxGroup("Modifier Settings")]
		public T source;

		[BoxGroup("Modifier Settings")]
		public T target;

		#endregion

		#region Properties

		public K modified => this.GetComponent(ref m_modified);

		#endregion

		#region Methods

		protected virtual void Awake()
		{
			if (!m_overrideSourceValue)
			{
				source = GetValue();
			}
			else if (m_overrideOnStart)
			{
				SetValue(source);
			}
		}

		private void OnEnable()
		{
			if (timedCurve != null)
			{
				timedCurve.onValueChanged.AddListener(OnValueChanged);
			}
		}

		private void OnDisable()
		{
			if (timedCurve != null)
			{
				timedCurve.onValueChanged.RemoveListener(OnValueChanged);
			}
		}

		private void OnValueChanged(float value)
		{
			SetValue(Lerp(value));
		}

		protected abstract T GetValue();
		protected abstract void SetValue(T value);
		protected abstract T Lerp(float value);

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		protected virtual bool ShowModified() => true;

#endif
		#endregion
	}
}