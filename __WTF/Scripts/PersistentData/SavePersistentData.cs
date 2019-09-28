namespace UnityEngine.Workshop
{
	public class SavePersistentData : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private bool m_saveOnAwake;

		#endregion

		#region Methods

		private void Start()
		{
			if (m_saveOnAwake)
			{
				Save();
			}
		}

		public void Save()
		{
			PersistentData.Save();
		}

		#endregion
	}
}
