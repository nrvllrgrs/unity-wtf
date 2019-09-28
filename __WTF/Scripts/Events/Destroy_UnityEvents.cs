using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class Destroy_UnityEvents : MonoBehaviour
	{
		#region Events

		[FoldoutGroup("Events")]
		public UnityEvent onDestroyed = new UnityEvent();

		#endregion

		#region Methods

		private void OnDestroy()
		{
			onDestroyed.Invoke();
		}

		#endregion
	}
}
