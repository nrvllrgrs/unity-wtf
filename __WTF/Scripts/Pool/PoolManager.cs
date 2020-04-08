using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	[ExecuteInEditMode]
	public class PoolManager : Singleton<PoolManager>
	{
		#region Variables

		[SerializeField]
		private List<PoolEntry> m_items;

		#endregion

		#region Propeties

		public IEnumerable<string> keys
		{
			get
			{
				return m_items.Where(x => !string.IsNullOrWhiteSpace(x.key))
					.Select(x => x.key)
					.OrderBy(x => x);
			}
		}

		#endregion

		#region Methods

		public void Add(PoolItem item)
		{
			PoolEntry entry;

			// Using autoKey and does not yet exist as an item...
			if (item.useAutoKey && !m_items.Any(x => Equals(x.key, item.key)))
			{
				entry = new PoolEntry(item.key, item.maxCount);
				m_items.Add(entry);
			}
			// Otherwise, find appropriate entry
			else
			{
				entry = m_items.FirstOrDefault(x => Equals(x.key, item.key));
			}
						
			if (entry == null)
				return;

			entry.Add(item);
		}

		public void Remove(PoolItem item)
		{
			var entry = m_items.FirstOrDefault(x => Equals(x.key, item.key));
			if (entry != null)
			{
				entry.Remove(item);
			}
		}

		public PoolItem Get(PoolItem item)
		{
			var entry = m_items.FirstOrDefault(x => Equals(x.key, item.key));
			if (entry == null)
			{
				Debug.LogErrorFormat("PoolItem {0} not found!", item.key);
				return null;
			}

			if (entry.count < entry.maxCount)
			{
				// Instantiate new entity
				Debug.LogFormat("Instantiating {0} pool item", item.key);
				var clone = Instantiate(item);

				if (entry.hideInHierarchy)
				{
					clone.gameObject.hideFlags |= HideFlags.HideInHierarchy;
				}

				return clone;
			}

			// Recycle existing entity
			return entry.Next();
		}

		public bool Contains(PoolItem item)
		{
			return m_items.FirstOrDefault(x => Equals(x.key, item.key)) != null;
		}

		public bool CanInstantiate(PoolItem item)
		{
			if (!item.useAutoKey)
				return false;

			var entry = m_items.FirstOrDefault(x => Equals(x.key, item.key));
			return entry == null || entry.count < entry.maxCount;
		}

		#endregion

		#region Structures

		[System.Serializable]
		public class PoolEntry
		{
			#region Variables

			[SerializeField]
			private string m_key;

			[SerializeField, MinValue(1)]
			private int m_maxCount = 1;

			[SerializeField]
			private bool m_canSteal = false;

			[SerializeField]
			private bool m_hideInHierarchy = false;

			// Oldest OR farthest

			private List<PoolItem> m_items = new List<PoolItem>();

			#endregion

			#region Properties

			public string key => m_key;
			public int count => m_items.Count;
			public int maxCount => m_maxCount;
			public bool hideInHierarchy => m_hideInHierarchy;

		#endregion

			#region Constructors

			public PoolEntry(string key, int maxCount)
			{
				m_key = key;
				m_maxCount = maxCount;
			}

			#endregion

			#region Methods

			public void Add(PoolItem item)
			{
				if (m_items.Count > maxCount)
				{
					var oldestItem = m_items[0];
					m_items.RemoveAt(0);

					Destroy(oldestItem.gameObject);
				}
				m_items.Add(item);
			}

			public void Remove(PoolItem item)
			{
				if (!m_items.Contains(item))
					return;

				m_items.Remove(item);
			}

			public PoolItem Next()
			{
				PoolItem item = m_canSteal
					? m_items.FirstOrDefault(x => x)
					: m_items.FirstOrDefault(x => !x.gameObject.activeInHierarchy);

				if (item == null)
					return null;

				// Remove from list
				m_items.Remove(item);

				// Add to end of list to indicate age
				m_items.Add(item);

				return item;
			}

			#endregion
		}

		#endregion
	}
}
