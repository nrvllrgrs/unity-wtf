using System.Collections.Generic;

namespace UnityEngine.Workshop
{
	public class IdentifierContext : Singleton<IdentifierContext>
	{
		#region Variables

		private Dictionary<int, GameObject> m_map = new Dictionary<int, GameObject>();

		#endregion

		#region Static Methods

		public static void Register(Identifier identifier)
		{
			if (identifier == null)
			{
				throw new System.ArgumentNullException("identifier");
			}

			Instance.m_map.Add(identifier.id, identifier.gameObject);
		}

		public static void Unregister(Identifier identifier)
		{
			if (identifier == null)
			{
				throw new System.ArgumentNullException("identifier");
			}

			Instance.m_map.Remove(identifier.id);
		}

		public static GameObject Get(int id)
		{
			return TryGet(id, out GameObject value)
				? value
				: null;
		}

		public static bool TryGet(int id, out GameObject value)
		{
			return Instance.m_map.TryGetValue(id, out value);
		}

		#endregion
	}
}
