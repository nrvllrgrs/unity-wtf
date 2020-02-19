namespace UnityEngine
{
	public class GameObejct_DestroyEvent : MonoBehaviour
	{
		#region Events

		public event System.EventHandler Destroyed;

		#endregion

		#region Methods

		private void OnDestroy()
		{
			Destroyed?.Invoke(this, System.EventArgs.Empty);
		}

		#endregion
	}
}