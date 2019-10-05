﻿using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public sealed class TimedCurve : MonoBehaviour, ITimedCurve
	{
		#region Variables

		public AnimationCurve curve;
		public WrapMode wrapMode;

		[MinValue(0f)]
		public float duration = 1f;

		[SerializeField]
		private bool m_playOnAwake;

		[SerializeField, ReadOnly]
		private bool m_isPlaying = false, m_isPaused = false, m_isReversed = false;
		private float m_remainingTime, m_value;

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

		public bool isPlaying
		{
			get { return m_isPlaying; }
			private set
			{
				if (isPlaying == value)
					return;

				// Toggle play and set value appropriately
				m_isPlaying = value;
				this.value = Evaluate(out float t);
				
				onPlayStatusChanged.Invoke(value);
			}
		}

		public bool isReversed => m_isReversed;

		public float value
		{
			get { return m_value; }
			private set
			{
				if (this.value == value)
					return;

				m_value = value;
				onValueChanged.Invoke(value);
			}
		}

		public float time => duration - Mathf.Clamp(m_remainingTime, 0f, duration);
		public float timePercent => time / duration;

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
		public void Play()
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
			if (!isPlaying)
			{
				m_remainingTime = duration;
			}

			Toggle(false);
		}

		[Button, FoldoutGroup("Editor")]
		public void Reverse()
		{
			if (!isPlaying)
			{
				m_remainingTime = 0f;
			}

			Toggle(true);
		}

		[Button, FoldoutGroup("Editor")]
		public void Toggle()
		{
			Toggle(!m_isReversed);
		}

		private void Toggle(bool isReversed)
		{
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
		public void StopAtBeginning()
		{
			m_isReversed = true;
			m_remainingTime = duration;

			if (isPlaying)
			{
				// Stop playing after time remaining to evaluate value correctly
				isPlaying = false;
			}
			else
			{
				value = Evaluate(out float t);
			}
		}

		[Button, FoldoutGroup("Editor")]
		public void StopAtEnd()
		{
			m_isReversed = false;
			m_remainingTime = 0f;

			if (isPlaying)
			{
				// Stop playing after time remaining to evaluate value correctly
				isPlaying = false;
			}
			else
			{
				value = Evaluate(out float t);
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
				m_remainingTime += Time.deltaTime;
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
