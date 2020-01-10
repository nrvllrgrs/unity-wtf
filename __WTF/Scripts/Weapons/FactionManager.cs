using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
#endif

namespace UnityEngine.Workshop
{
	public class FactionManager : Singleton<FactionManager>
	{
		#region Variables

		[SerializeField, InlineEditor]
		private FactionDatabase m_database;

		#endregion

		#region Methods

		public string[] GetFactionNames()
		{
			return m_database != null
				? m_database.GetFactionNames()
				: null;
		}

		public float GetDamageFactor(Faction a, Faction b)
		{
			if (m_database == null || a == null || b == null)
			{
				return 1f;
			}

			if (a.key == b.key && m_database.GetFaction(a.key).isPlayerFaction)
			{
				return m_database.GetPlayerFactionDamageFactor();
			}

			return m_database.GetDamageFactor(m_database.GetRelationship(a, b));
		}

		#if UNITY_EDITOR
		protected override void Refresh()
		{
			m_database.Refresh();
		}
		#endif

#endregion
    }
}