using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	[ExecuteInEditMode]
	public class ArmorManager : Singleton<ArmorManager>
	{
		#region Variables

		[SerializeField, Required, InlineEditor]
		private ArmorDatabase m_database;

		#endregion

		#region Properties

		public static bool Ready => !Application.isPlaying || Exists;

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
