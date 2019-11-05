using System.Linq;
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
		private DamageTypeInfo[] m_damageTypes;

		#endregion

		#region Methods

		public string[] GetDamageTypes() => m_damageTypes.Select(x => x.name).ToArray();

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
			#region Variables

			[SerializeField]
			private string m_name;

			[Header("Status Effect Settings")]

			[SerializeField]
			private StatusEffectFactory m_statusEffectFactory;

			[SerializeField, Range(0f, 1f)]
			private float m_percent = 1f;

			#endregion

			#region Properties

			public string name => m_name;
			public StatusEffectFactory statusEffectFactory => m_statusEffectFactory;
			public float percent => m_percent;

			#endregion
		}

		#endregion
	}
}
