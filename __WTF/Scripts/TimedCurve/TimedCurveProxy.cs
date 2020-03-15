using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class TimedCurveProxy : BaseTimedCurve
	{
		#region Variables

		[SerializeField]
		private string m_key;

		private TimedCurve m_timedCurve;

		#endregion

		#region Events

		[SerializeField, FoldoutGroup("Events")]
		private UnityEvent m_onStarted = new UnityEvent();

		[SerializeField, FoldoutGroup("Events")]
		private SingleEvent m_onValueChanged = new SingleEvent();

		[SerializeField, FoldoutGroup("Events")]
		private UnityEvent m_onStopped = new UnityEvent();

		[SerializeField, FoldoutGroup("Events")]
		private UnityEvent m_onBeginningReached = new UnityEvent();

		[SerializeField, FoldoutGroup("Events")]
		private UnityEvent m_onEndReached = new UnityEvent();

		#endregion

		#region Properties

		public override UnityEvent onStarted => m_onStarted;
		public override SingleEvent onValueChanged => m_onValueChanged;
		public override UnityEvent onStopped => m_onStopped;
		public override UnityEvent onBeginningReached => m_onBeginningReached;
		public override UnityEvent onEndReached => m_onEndReached;

		public override bool isPlaying
		{
			get => m_timedCurve != null ? m_timedCurve.isPlaying : false;
			protected set => throw new System.NotImplementedException();
		}

		public override float value
		{
			get => m_timedCurve != null ? m_timedCurve.value : 0f;
			protected set => throw new System.NotImplementedException();
		}

		public override bool isReversed => m_timedCurve != null ? m_timedCurve.isReversed : false;
		public override float time => m_timedCurve != null ? m_timedCurve.time : 0f;
		public override float timePercent => m_timedCurve != null ? m_timedCurve.timePercent : 0f;

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

			m_timedCurve = TimedCurveManager.Instance.Get(m_key);
			m_timedCurve.onStarted.AddListener(TimedCurveStarted);
			m_timedCurve.onValueChanged.AddListener(ValueChanged);
			m_timedCurve.onStopped.AddListener(TimedCurveStopped);
			m_timedCurve.onBeginningReached.AddListener(TimedCurveBeginningReached);
			m_timedCurve.onEndReached.AddListener(TimedCurveEndReached);
		}

		private void Unregister()
		{
			if (!TimedCurveManager.Instance.ContainsKey(m_key))
				return;

			m_timedCurve = TimedCurveManager.Instance.Get(m_key);
			m_timedCurve.onStarted.RemoveListener(TimedCurveStarted);
			m_timedCurve.onValueChanged.RemoveListener(ValueChanged);
			m_timedCurve.onStopped.RemoveListener(TimedCurveStopped);
			m_timedCurve.onBeginningReached.RemoveListener(TimedCurveBeginningReached);
			m_timedCurve.onEndReached.RemoveListener(TimedCurveEndReached);
		}

		private void TimedCurveStarted()
		{
			onStarted.Invoke();
		}

		private void ValueChanged(float value)
		{
			if (enabled)
			{
				onValueChanged.Invoke(value);
			}
		}

		private void TimedCurveStopped()
		{
			onStopped.Invoke();
		}

		private void TimedCurveBeginningReached()
		{
			onBeginningReached.Invoke();
		}

		private void TimedCurveEndReached()
		{
			onEndReached.Invoke();
		}

		public void Play()
		{
			enabled = true;
		}

		public void StopAtBeginning()
		{
			StopAtTime(0f);
		}

		public void StopAtEnd()
		{
			StopAtTime(1f);
		}

		public void StopAtTime(float t)
		{
			enabled = false;
			if (m_timedCurve != null)
			{
				onValueChanged.Invoke(m_timedCurve.curve.Evaluate(t));
			}
		}

		#endregion
	}
}