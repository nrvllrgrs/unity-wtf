using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public sealed class TimedCurve : BaseTimedCurve
	{
		#region Variables

		public AnimationCurve curve;
		public WrapMode wrapMode;

		[MinValue(0f)]
		public float duration = 1f;

		[MinValue(0f)]
		public float reverseDurationFactor = 1f;

		[SerializeField]
		private bool m_playOnAwake;

		[SerializeField, ReadOnly]
		private bool m_isPlaying = false, m_isPaused = false, m_isReversed = false;
		private float m_remainingTime, m_value;

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
			get { return m_isPlaying; }
			protected set
			{
				if (isPlaying == value)
					return;

				// Toggle play and set value appropriately
				m_isPlaying = value;
				this.value = Evaluate(out float t);
				
				if (value)
				{
					onStarted.Invoke();
				}
				else
				{
					onStopped.Invoke();
				}
			}
		}

		public override bool isReversed => m_isReversed;

		public override float value
		{
			get { return m_value; }
			protected set
			{
				if (this.value == value)
					return;

				m_value = value;
				onValueChanged.Invoke(value);

				if (Mathf.Approximately(timePercent, 0f))
				{
					onBeginningReached?.Invoke();
				}
				else if (Mathf.Approximately(timePercent, 1f))
				{
					onEndReached?.Invoke();
				}
			}
		}

		public override float time => duration - Mathf.Clamp(m_remainingTime, 0f, duration);
		public override float timePercent => time / duration;

		#endregion

		#region Methods

		private void Start()
		{
			if (m_playOnAwake)
			{
				Play();
			}
		}

		[Button, FoldoutGroup("Editor")]
		public override void Play()
		{
			m_remainingTime = duration;
			m_isReversed = false;
			isPlaying = true;
		}

		[Button, FoldoutGroup("Editor")]
		public void Pause()
		{
			m_isPaused = true;
		}

		[Button, FoldoutGroup("Editor")]
		public void Unpause()
		{
			m_isPaused = false;
		}

		[Button, FoldoutGroup("Editor")]
		public void Forward()
		{
			Toggle(false);
		}

		[Button, FoldoutGroup("Editor")]
		public void Reverse()
		{
			Toggle(true);
		}

		[Button, FoldoutGroup("Editor")]
		public void Toggle()
		{
			Toggle(!m_isReversed);
		}

		private void Toggle(bool isReversed)
		{
			if (!isPlaying)
			{
				m_remainingTime = !isReversed ? duration : 0f;
			}

			m_isReversed = isReversed;
			isPlaying = true;
		}

		[Button, FoldoutGroup("Editor")]
		public void Stop()
		{
			// Stop immediately, value will remaining as is
			isPlaying = false;
		}

		[Button, FoldoutGroup("Editor")]
		public override void StopAtBeginning()
		{
			m_isReversed = true;
			StopAtTime(0f);
		}

		[Button, FoldoutGroup("Editor")]
		public override void StopAtEnd()
		{
			m_isReversed = false;
			StopAtTime(1f);
		}

		[Button, FoldoutGroup("Editor")]
		public override void StopAtTime(float t)
		{
			m_remainingTime = duration - (duration * t);

			if (isPlaying)
			{
				// Stop playing after time remaining to evaluate value correctly
				isPlaying = false;
			}
			else
			{
				value = Evaluate(out float time);
			}
		}

		private void Update()
		{
			if (!isPlaying || m_isPaused)
				return;

			value = Evaluate(out float t);
			//Debug.LogFormat("Value = {0}; T = {1}; Remaining Time = {2}", value, t, m_remainingTime);

			if (!m_isReversed && t < 1f)
			{
				m_remainingTime -= Time.deltaTime;
			}
			else if (m_isReversed && t > 0f)
			{
				m_remainingTime += Time.deltaTime * reverseDurationFactor;
			}
			else
			{
				switch (wrapMode)
				{
					case WrapMode.Loop:
						m_remainingTime = !m_isReversed ? duration : 0f;
						break;

					case WrapMode.PingPong:
						Toggle();
						break;

					// Let it keep playing at the end value
					case WrapMode.Clamp:
						break;

					case WrapMode.ClampForever:
						enabled = false;
						break;

					default:
						isPlaying = false;
						break;
				}
			}
		}

		private float Evaluate(out float t)
		{
			if (curve == null)
			{
				t = 1f;
				return 0f;
			}

			t = duration > 0f
				? Mathf.Clamp01(1f - m_remainingTime / duration)
				: 1f;
			return curve.Evaluate(t);
		}

		#endregion
	}
}
