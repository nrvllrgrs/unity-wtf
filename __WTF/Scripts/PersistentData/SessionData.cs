using System.Collections.Generic;

namespace UnityEngine.Workshop
{
	public static class SessionData
	{
		#region Variables

		private static Dictionary<string, object> s_data;

		#endregion

		#region Methods

		public static bool TryGet<T>(string key, out T value)
		{
			bool result = s_data.TryGetValue(key, out object rawValue);
			value = (T)rawValue;
			return result;
		}

		public static T Get<T>(string key, T fallback)
		{
			if (TryGet(key, out T value))
			{
				return value;
			}

			Set(key, fallback);
			return fallback;
		}

		public static void Set<T>(string key, T value)
		{
			s_data.Set(key, value);
		}

		public static bool Exists(string key)
		{
			return s_data.ContainsKey(key);
		}

		#endregion
	}
}
