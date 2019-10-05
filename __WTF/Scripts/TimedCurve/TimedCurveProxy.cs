using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class TimedCurveProxy : MonoBehaviour, ITimedCurve
	{
		#region Variables

		[SerializeField]
		private string m_key;

		#endregion

		#region Events

		[SerializeField, FoldoutGroup("Events")]
		private BoolEvent m_onPlayStatusChanged = new BoolEvent();

		[SerializeField, FoldoutGroup("Events")]
		private SingleEvent m_onValueChanged = new SingleEvent();

		#endregion

		#region Properties

		public BoolEvent onPlayStatusChanged => m_onPlayStatusChanged;
		public SingleEvent onValueChanged => m_onValueChanged;

		#endregion

		#region Methods

		private void OnEnable()
		{
			this.WaitUntil(
				() => { return TimedCurveManager.Exists; },
				() => { Register(); });
		}

		private void OnDisable()
		{
			if (TimedCurveManager.Exists)
			{
				Unregister();
			}
		}

		private void Register()
		{
			if (!TimedCurveManager.Instance.ContainsKey(m_key))
				return;

			var timedCurve = TimedCurveManager.Instance.Get(m_key);
			timedCurve.onPlayStatusChanged.AddListener(PlayStatusChanged);
			timedCurve.onValueChanged.AddListener(ValueChanged);
		}

		private void Unregister()
		{
			if (!TimedCurveManager.Instance.ContainsKey(m_key))
				return;

			var timedCurve = TimedCurveManager.Instance.Get(m_key);
			timedCurve.onPlayStatusChanged.RemoveListener(PlayStatusChanged);
			timedCurve.onValueChanged.RemoveListener(ValueChanged);
		}

		private void PlayStatusChanged(bool isOn)
		{
			onPlayStatusChanged.Invoke(isOn);
		}

		private void ValueChanged(float value)
		{
			onValueChanged.Invoke(value);
		}

		#endregion
	}
}