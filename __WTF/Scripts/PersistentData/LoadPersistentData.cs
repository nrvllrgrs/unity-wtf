using UnityEngine;

namespace UnityEngine.Workshop
{
	public class LoadPersistentData : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private bool m_loadOnAwake;

		#endregion

		#region Methods

		private void Start()
		{
			if (m_loadOnAwake)
			{
				Load();
			}
		}

		public void Load()
		{
			PersistentData.Load();
		}

		#endregion
	}
}
