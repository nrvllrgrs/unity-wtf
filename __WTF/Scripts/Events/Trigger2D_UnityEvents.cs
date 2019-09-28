using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	[System.Serializable]
	public class Trigger2DEvent : UnityEvent<Collider2D>
	{ }

	public class Trigger2D_UnityEvents : MonoBehaviour
	{
		#region Events

		[FoldoutGroup("Events")]
		public Trigger2DEvent onTriggerEnter = new Trigger2DEvent();

		[FoldoutGroup("Events")]
		public Trigger2DEvent onTriggerStay = new Trigger2DEvent();

		[FoldoutGroup("Events")]
		public Trigger2DEvent onTriggerExit = new Trigger2DEvent();

		#endregion

		#region Methods

		private void OnTriggerEnter2D(Collider2D other)
		{
			onTriggerEnter.Invoke(other);
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			onTriggerStay.Invoke(other);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			onTriggerExit.Invoke(other);
		}

		#endregion
	}
}