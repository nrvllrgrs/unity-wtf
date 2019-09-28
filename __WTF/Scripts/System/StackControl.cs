using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public abstract class StackControl<T> : SerializedMonoBehaviour
	{
		#region Variables

		[SerializeField, Required]
		protected StackRule<T> m_rule;

		private List<T> m_items = new List<T>();

		#endregion

		#region Properties

		public bool isEmpty => !m_items.Any();

		#endregion

		#region Methods

		public void Push(T item)
		{
			// Remember previous top item
			bool wasTopValid = TryGetTop(out T pre);

			// Add new item to stack
			m_items.Add(item);

			if (m_rule != null)
			{
				m_rule.Pushed(pre, item);
			}
		}

		public bool Pop(out T item)
		{
			if (isEmpty)
			{
				item = default;
				return false;
			}

			// Remember item at top and remove from stack
			TryGetTop(out item);
			m_items.RemoveAt(m_items.Count - 1);

			if (m_rule != null)
			{
				bool isTopValid = TryGetTop(out T post);
				m_rule.Popped(item, post);
			}

			return true;
		}

		public bool TryGetTop(out T item)
		{
			if (isEmpty)
			{
				item = default;
				return false;
			}

			item = m_items[m_items.Count - 1];
			return true;
		}

		#endregion
	}

	public abstract class StackRule<T> : MonoBehaviour
	{
		#region Methods

		public abstract void Pushed(T pre, T post);
		public abstract void Popped(T pre, T post);

		#endregion
	}
}
