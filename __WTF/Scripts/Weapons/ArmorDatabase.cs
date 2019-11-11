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

		public string[] GetDamageTypes() => new[] { string.Empty }.Concat(m_damageTypes.Select(x => x.name)).ToArray();

		public bool TryGetStatusEffect(string damageType, out string statusEffect, out float percent)
		{
#if STATUS_EFFECT

			var info = m_damageTypes.SingleOrDefault(x => x.name == damageType);
			if (info != null)
			{
				statusEffect = info.statusEffect;
				percent = info.percent;
				return true;
			}

#endif

			statusEffect = null;
			percent = 0f;
			return false;
		}

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

#if STATUS_EFFECT

			[Header("Status Effect Settings")]

			[SerializeField, ValueDropdown("GetStatusEffectNames")]
			private string m_statusEffect;

			[SerializeField, Range(0f, 1f)]
			private float m_percent = 1f;

#endif
			#endregion

			#region Properties

			public string name => m_name;

#if STATUS_EFFECT

			public string statusEffect => m_statusEffect;
			public float percent => m_percent;

#endif
			#endregion

			#region Editor Methods
#if UNITY_EDITOR

			private string[] GetStatusEffectNames()
			{
				return StatusEffectManager.Ready
					? StatusEffectManager.Instance.GetStatusEffectNames()
					: new string[] { };
			}

#endif
			#endregion
		}

		#endregion
	}
}
