using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Workshop
{
	public class SetCollection : Set
	{
		#region 

		[SerializeField]
		private List<Set> m_sets = new List<Set>();

		#endregion

		#region Methods

		public override void Add(GameObject item, bool autoRemoveOnDestroy = true)
		{
			base.Add(item, autoRemoveOnDestroy);

			// Add item to every existing set
			foreach (var set in m_sets)
			{
				if (set != null)
				{
					set.Add(item, autoRemoveOnDestroy);
				}
			}
		}

		public override void Remove(GameObject item)
		{
			base.Remove(item);

			// Remove item from every existing set
			foreach (var set in m_sets)
			{
				if (set != null)
				{
					set.Remove(item);
				}
			}
		}

		#endregion
	}
}
