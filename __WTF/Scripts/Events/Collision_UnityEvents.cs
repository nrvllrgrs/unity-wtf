using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	[System.Serializable]
	public class CollisionEvent : UnityEvent<Collision>
	{ }

	public class Collision_UnityEvents : MonoBehaviour
	{
		#region Events

		[FoldoutGroup("Events")]
		public CollisionEvent onCollisionEnter = new CollisionEvent();

		[FoldoutGroup("Events")]
		public CollisionEvent onCollisionStay = new CollisionEvent();

		[FoldoutGroup("Events")]
		public CollisionEvent onCollisionExit = new CollisionEvent();

		#endregion

		#region Methods

		private void OnCollisionEnter(Collision collision)
		{
			onCollisionEnter.Invoke(collision);
		}

		private void OnCollisionStay(Collision collision)
		{
			onCollisionStay.Invoke(collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			onCollisionExit.Invoke(collision);
		}

		#endregion
	}
}