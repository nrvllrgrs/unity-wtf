namespace UnityEngine.Workshop
{
	public interface ITimedCurve
	{
		#region Properties

		BoolEvent onPlayStatusChanged { get; }
		SingleEvent onValueChanged { get; }

		#endregion
	}
}