using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class ArmorManager : Singleton<ArmorManager>
	{
		#region Variables

		[SerializeField, Required, InlineEditor]
		private ArmorDatabase m_database;

		#endregion

		#region Methods

		public string[] GetDamageTypes()
		{
			return m_database != null
				? m_database.GetDamageTypes()
				: null;
		}

		public bool TryGetStatusEffect(string damageType, out string statusEffect, out float percent)
		{
			if (m_database == null)
			{
				statusEffect = null;
				percent = 0f;
				return false;
			}

			return m_database.TryGetStatusEffect(damageType, out statusEffect, out percent);
		}

		#endregion
	}
}
