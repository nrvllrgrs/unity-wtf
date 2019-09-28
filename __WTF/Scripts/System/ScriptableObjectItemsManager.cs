using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Groundling
{
	public abstract class ScriptableObjectItemsManager<T, I, J, K> : Singleton<T>
		where T : MonoBehaviour
		where J : IEnumerable<K>
	{
		#region Variables

		[SerializeField, Required, InlineEditor, HideLabel]
		protected J m_data;

		private List<I> m_items;

		#endregion

		#region Properties

		public IEnumerable<I> items { get { return m_items; } }
		public abstract string persistentDataKey { get; }

		#endregion

		#region Methods

		protected override void Awake()
		{
			base.Awake();

			m_items = new List<I>();

			// Create instanced data
			foreach (var item in m_data)
			{
				m_items.Add(LoadItemData(item));
			}
		}

		protected abstract I LoadItemData(K dataItem);

		public virtual string GetPersistentDataFormattedKey(K dataItem)
		{
			return string.Format("{1}:id{0}", dataItem.GetHashCode(), persistentDataKey);
		}

		#endregion
	}
}