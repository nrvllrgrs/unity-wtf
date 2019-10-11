namespace UnityEngine
{
	public class GameObject_UpdateEvent : MonoBehaviour
	{
		#region Events

		public event System.EventHandler OnUpdate;

		#endregion

		#region Methods

		private void Update()
		{
			OnUpdate?.Invoke(this, System.EventArgs.Empty);
		}

		#endregion
	}
}