using UnityEngine;

#if UNITY_EDITOR
#endif

namespace UnityEngine.Workshop
{
	public class DontDestroyOnLoad : MonoBehaviour
	{
		#region Methods

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		#endregion
	}
}
