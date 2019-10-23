using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Workshop
{
	public class ArmorDatabase : SerializedScriptableObject
	{
		#region Variables

		[SerializeField]
		private string[] m_damageTypes;

		#endregion

		#region Methods

		public string[] GetDamageTypes()
		{
			return m_damageTypes;
		}

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		[MenuItem("Assets/Create/Workshop/Armor Database")]
		private static void CreateAsset()
		{
			var data = UnityUtil.CreateAsset<ArmorDatabase>();
		}

#endif
		#endregion
	}
}
