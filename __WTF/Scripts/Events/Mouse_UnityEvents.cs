using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public sealed class Mouse_UnityEvents : MonoBehaviour
	{
		#region Events

		[FoldoutGroup("Events")]
		public UnityEvent onMouseDown = new UnityEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onMouseDrag = new UnityEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onMouseEnter = new UnityEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onMouseExit = new UnityEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onMouseOver = new UnityEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onMouseUp = new UnityEvent();

		#endregion

		#region Methods

		private void OnMouseDown()
		{
			onMouseDown.Invoke();
		}

		private void OnMouseDrag()
		{
			onMouseDrag.Invoke();
		}

		private void OnMouseEnter()
		{
			onMouseEnter.Invoke();
		}

		private void OnMouseExit()
		{
			onMouseExit.Invoke();
		}

		private void OnMouseOver()
		{
			onMouseOver.Invoke();
		}

		private void OnMouseUp()
		{
			onMouseUp.Invoke();
		}

		#endregion
	}
}
