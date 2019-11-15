namespace UnityEngine.Workshop
{
	public abstract class BaseTimedCurve : MonoBehaviour
	{
		#region Properties

		public abstract BoolEvent onPlayStatusChanged { get; }
		public abstract SingleEvent onValueChanged { get; }

		public abstract bool isPlaying { get; protected set; }
		public abstract bool isReversed { get; }
		public abstract float value { get; protected set; }
		public abstract float time { get; }
		public abstract float timePercent { get; }

		#endregion
	}
}