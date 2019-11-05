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

		public string[] GetDamageTypes() => m_damageTypes;

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		[MenuItem("Assets/Create/Workshop/Armor Database")]
		private static void CreateAsset()
		{
			UnityUtil.CreateAsset<ArmorDatabase>();
		}

#endif
		#endregion

		#region Structures

		[System.Serializable]
		public class DamageTypeInfo
		{
			public string name;

			public StatusEffectFactory statusEffectFactory;

			[Range(0f, 1f)]
			public float percent = 1f;
		}

		#endregion
	}
}
