using UnityEngine;

namespace UnityEngine.Workshop
{
	public class TimedCurveManager : Singleton<TimedCurveManager>
	{
		#region Variables

		[SerializeField]
		private TimedCurveMap m_map;

		#endregion

		#region Methods

		public bool ContainsKey(string key)
		{
			return m_map.ContainsKey(key);
		}

		public TimedCurve Get(string key)
		{
			return m_map.Get(key);
		}

		#endregion
	}
}