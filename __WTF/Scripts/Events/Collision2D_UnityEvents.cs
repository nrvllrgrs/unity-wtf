using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	[System.Serializable]
	public class Collision2DEvent : UnityEvent<Collision2D>
	{ }

	public class Collision2D_UnityEvents : MonoBehaviour
	{
		#region Events

		[FoldoutGroup("Events")]
		public Collision2DEvent onCollisionEnter2D = new Collision2DEvent();

		[FoldoutGroup("Events")]
		public Collision2DEvent onCollisionStay2D = new Collision2DEvent();

		[FoldoutGroup("Events")]
		public Collision2DEvent onCollisionExit2D = new Collision2DEvent();

		#endregion

		#region Methods

		private void OnCollisionEnter2D(Collision2D collision)
		{
			onCollisionEnter2D.Invoke(collision);
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			onCollisionStay2D.Invoke(collision);
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			onCollisionExit2D.Invoke(collision);
		}

		#endregion
	}
}