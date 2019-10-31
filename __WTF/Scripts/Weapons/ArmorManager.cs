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

		#endregion
	}
}
