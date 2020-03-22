using UnityEngine.Events;

namespace UnityEngine.Workshop
{
	public abstract class BaseTimedCurve : MonoBehaviour
	{
		#region Properties

		public abstract UnityEvent onStarted { get; }
		public abstract SingleEvent onValueChanged { get; }
		public abstract UnityEvent onStopped { get; }
		public abstract UnityEvent onBeginningReached { get; }
		public abstract UnityEvent onEndReached { get; }

		public abstract bool isPlaying { get; protected set; }
		public abstract bool isReversed { get; }
		public abstract float value { get; protected set; }
		public abstract float time { get; }
		public abstract float timePercent { get; }

		#endregion

		#region Methods

		public abstract void Play();
		public abstract void StopAtBeginning();
		public abstract void StopAtEnd();
		public abstract void StopAtTime(float t);

		#endregion
	}
}