using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class Faction : MonoBehaviour
	{
		#region Enumerators

		public enum RelationshipType
		{
			Neutral,
			Friend,
			Enemy
		}

		#endregion

		#region Variables

		[SerializeField, ValueDropdown("GetFactionNames")]
		private string m_key;

		#endregion

		#region Properties

		public string key => m_key;

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		private string[] GetFactionNames()
		{
			return FactionManager.Exists
				? FactionManager.Instance.GetFactionNames()
				: new string[] { };
		}

#endif
		#endregion
	}
}