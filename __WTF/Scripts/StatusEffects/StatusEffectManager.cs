using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Workshop
{
	public class StatusEffectManager : Singleton<StatusEffectManager>
	{
		#region Variables

		[SerializeField]
		private List<StatusEffectInfo> m_statusEffects;

		#endregion

		#region Methods

		public StatusEffect GetStatusEffect(string key, GameObject target)
		{
			return m_statusEffects?.SingleOrDefault(x => x.statusEffect.key == key)
				?.statusEffect
				?.GetStatusEffect(target);
		}

		public string[] GetStatusEffectNames()
		{
			return m_statusEffects != null
				? new[] { string.Empty }.Concat(m_statusEffects.Select(x => x.statusEffect.key)).ToArray()
				: new string[] { };
		}

		#endregion

		#region Structures

		[System.Serializable]
		public class StatusEffectInfo
		{
			#region Variables

			[SerializeField]
			public StatusEffectFactory m_statusEffect;

			#endregion

			#region Properties

			public StatusEffectFactory statusEffect => m_statusEffect;

			#endregion
		}

		#endregion
	}
}