using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class TimedCurveMap : SerializedMonoBehaviour
	{
		#region Variables

		[SerializeField]
		private Dictionary<string, TimedCurve> m_items = new Dictionary<string, TimedCurve>();

		#endregion

		#region Methods

		public bool ContainsKey(string key)
		{
			return m_items.ContainsKey(key);
		}

		public TimedCurve Get(string key)
		{
			return m_items[key];
		}

		#endregion
	}
}