using System.Collections.Generic;

namespace UnityEngine.AI
{
	public static class NavMeshUtil
	{
		#region Fields

		private static Dictionary<string, int> s_agentTypeMap;

		#endregion

		#region Methods

		public static string[] GetAgentTypeNames()
		{
			int count = NavMesh.GetSettingsCount();
			var agentTypeNames = new string[count + 2];

			for (int i = 0; i < count; ++i)
			{
				int id = NavMesh.GetSettingsByIndex(i).agentTypeID;
				string name = NavMesh.GetSettingsNameFromID(id);
				agentTypeNames[i] = name;
			}

			return agentTypeNames;
		}

		public static int GetAgentTypeId(string agentType, bool forceReset = false)
		{
			if (string.IsNullOrWhiteSpace(agentType))
				return 0;

			if (forceReset || s_agentTypeMap == null)
			{
				ResetAgentTypeIds();
			}

			return s_agentTypeMap.TryGetValue(agentType, out int value)
				? value
				: 0;
		}

		public static void ResetAgentTypeIds()
		{
			s_agentTypeMap = new Dictionary<string, int>();

			int count = NavMesh.GetSettingsCount();
			for (int i = 0; i < count; ++i)
			{
				int id = NavMesh.GetSettingsByIndex(i).agentTypeID;
				string name = NavMesh.GetSettingsNameFromID(id);
				s_agentTypeMap.Add(name, id);
			}
		}

		#endregion
	}
}
