using UnityEngine;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public sealed class EnableDisable_UnityEvents : MonoBehaviour
	{
		#region Events

		[FoldoutGroup("Events")]
		public GameObjectEvent onEnabled = new GameObjectEvent();

		[FoldoutGroup("Events")]
		public GameObjectEvent onDisabled = new GameObjectEvent();

		#endregion

		#region Methods

		private void OnEnable()
		{
			onEnabled.Invoke(gameObject);
		}

		private void OnDisable()
		{
			onDisabled.Invoke(gameObject);
		}

		#endregion
	}
}