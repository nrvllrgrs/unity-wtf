using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class Lifetime : MonoBehaviour
	{
		#region Variables

		[SerializeField, MinValue(0f)]
		private float m_time;

		#endregion

		#region Methods

		private void OnEnable()
		{
			GameObjectUtil.Destroy(gameObject, m_time);
		}

		#endregion
	}
}
