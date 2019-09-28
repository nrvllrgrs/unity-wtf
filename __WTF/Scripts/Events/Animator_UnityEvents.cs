using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	[System.Serializable]
	public class AnimatorIKEvent : UnityEvent<int>
	{ }

	public class Animator_UnityEvents : MonoBehaviour
	{
		#region Events

		[FoldoutGroup("Events")]
		public AnimatorIKEvent onAnimatorIK = new AnimatorIKEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onAnimatorMove = new UnityEvent();

		#endregion

		#region Methods

		private void OnAnimatorIK(int layerIndex)
		{
			onAnimatorIK.Invoke(layerIndex);
		}

		private void OnAnimatorMove()
		{
			onAnimatorMove.Invoke();
		}

		#endregion
	}
}
