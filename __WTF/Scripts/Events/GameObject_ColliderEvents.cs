namespace UnityEngine
{
	public delegate void CollisionEventHandler(object sender, CollisionEventArgs e);

	[Ludiq.IncludeInSettings(true)]
	public class CollisionEventArgs : System.EventArgs
	{
		#region Variables

		private readonly Collision m_collision;

		#endregion

		#region Properties

		public Collider collider => m_collision.collider;
		public ContactPoint[] contacts => m_collision.contacts;
		public Vector3 impulse => m_collision.impulse;
		public Vector3 relativeVelocity => m_collision.relativeVelocity;

		#endregion

		#region Constructors

		public CollisionEventArgs(Collision collision)
		{
			m_collision = collision;
		}

		#endregion
	}

	public class GameObject_ColliderEvents : MonoBehaviour
	{
		#region Events

		public event CollisionEventHandler CollisionEnter;
		public event CollisionEventHandler CollisionStay;
		public event CollisionEventHandler CollisionExit;

		#endregion

		#region Methods

		private void OnCollisionEnter(Collision collision)
		{
			CollisionEnter?.Invoke(this, new CollisionEventArgs(collision));
		}

		private void OnCollisionStay(Collision collision)
		{
			CollisionStay?.Invoke(this, new CollisionEventArgs(collision));
		}

		private void OnCollisionExit(Collision collision)
		{
			CollisionExit?.Invoke(this, new CollisionEventArgs(collision));
		}

		#endregion
	}
}