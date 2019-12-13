namespace UnityEngine
{
	public delegate void TriggerEventHandler(object sender, TriggerEventArgs e);

	[Ludiq.IncludeInSettings(true)]
	public class TriggerEventArgs : System.EventArgs
	{
		#region Properties

		public Collider collider { get; private set; }

		#endregion

		#region Constructors

		public TriggerEventArgs(Collider collider)
		{
			this.collider = collider;
		}

		#endregion
	}

	public class GameObject_TriggerEvents : MonoBehaviour
	{
		#region Events

		public event TriggerEventHandler TriggerEnter;
		public event TriggerEventHandler TriggerStay;
		public event TriggerEventHandler TriggerExit;

		#endregion

		#region Methods

		private void OnTriggerEnter(Collider other)
		{
			TriggerEnter?.Invoke(this, new TriggerEventArgs(other));
		}

		private void OnTriggerStay(Collider other)
		{
			TriggerStay?.Invoke(this, new TriggerEventArgs(other));
		}

		private void OnTriggerExit(Collider other)
		{
			TriggerExit?.Invoke(this, new TriggerEventArgs(other));
		}

		#endregion
	}
}