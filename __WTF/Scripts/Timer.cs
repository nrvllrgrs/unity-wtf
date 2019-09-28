using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class Timer : MonoBehaviour
	{
		#region Variables

		[SerializeField, Tooltip("Seconds remaining in timer")]
		private float m_remainingTime = 60f;

		[SerializeField]
		private bool m_playOnAwake = false;

		[SerializeField, ReadOnly]
		private bool m_isPlaying, m_isPaused;

		private float m_initRemainingTime;

		#endregion

		#region Events

		[FoldoutGroup("Events")]
		public BoolEvent onPlayStatusChanged = new BoolEvent();

		[FoldoutGroup("Events")]
		public BoolEvent onPauseStatusChanged = new BoolEvent();

		[FoldoutGroup("Events")]
		public SingleEvent onRemainingTimeChanged = new SingleEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onTimeExpired = new UnityEvent();

		#endregion

		#region Properties

		public float remainingTime
		{
			get { return m_remainingTime; }
			private set
			{
				value = Mathf.Clamp(value, 0f, m_initRemainingTime);
				if (value == remainingTime)
					return;

				m_remainingTime = value;
				onRemainingTimeChanged.Invoke(value);

				if (value <= 0f)
				{
					onTimeExpired.Invoke();
					isPlaying = false;
				}
			}
		}

		public System.TimeSpan remainingTimeSpan
		{
			get { return new System.TimeSpan(0, 0, 0, 0, (int)(remainingTime * 1000)); }
		}

		public float remainingPercent
		{
			get { return UnityUtil.GetPercent(remainingTime, 0f, m_initRemainingTime); }
		}

		public bool isPlaying
		{
			get { return m_isPlaying; }
			set
			{
				if (value == isPlaying)
					return;

				// When toggled, restart timer
				m_remainingTime = m_initRemainingTime;

				m_isPlaying = value;
				onPlayStatusChanged.Invoke(value);
			}
		}

		public bool isPaused
		{
			get { return m_isPaused; }
			set
			{
				if (value == isPaused)
					return;

				m_isPaused = value;
				onPauseStatusChanged.Invoke(value);
			}
		}

		#endregion

		#region Methods

		private void Start()
		{
			// Remember initial value
			m_initRemainingTime = m_remainingTime;

			if (m_playOnAwake)
			{
				isPlaying = true;
			}
		}

		private void Update()
		{
			if (!isPlaying || isPaused)
				return;

			remainingTime -= Time.deltaTime;
		}

		public void Set(float remainingTime)
		{
			isPlaying = false;
			m_initRemainingTime = remainingTime;
		}

		#endregion
	}
}
