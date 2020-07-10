using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Workshop;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	#region Components

	[System.Serializable]
	public class SetEvent : UnityEvent<GameObject>
	{ }

	public class Set : MonoBehaviour, ISet
	{
		#region Variables

		private SetLogic m_set = new SetLogic();

		#endregion

		#region Properties

		public int count { get => m_set.count; }
		public bool isEmpty { get => count == 0; }

		[ShowInInspector, ReadOnly]
		public GameObject[] items { get => m_set.items; }

		#endregion

		#region Events

		[FoldoutGroup("Events")]
		public SetEvent onItemAdded = new SetEvent();

		[FoldoutGroup("Events")]
		public SetEvent onItemRemoved = new SetEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onEmpty = new UnityEvent(); 

		#endregion

		#region Methods

		private void Awake()
		{
			m_set.ItemAdded += (object sender, SetLogicEventArgs e) =>
			{
				onItemAdded.Invoke(e.item);
			};

			m_set.ItemRemoved += (object sender, SetLogicEventArgs e) =>
			{
				onItemRemoved.Invoke(e.item);

				if (m_set.count == 0)
				{
					onEmpty.Invoke();
				}
			};
		}

		public virtual void Add(GameObject item, bool autoRemoveOnDestroy = true)
		{
			m_set.Add(item, autoRemoveOnDestroy);
		}

		public virtual void Remove(GameObject item)
		{
			m_set.Remove(item);
		}

		public virtual void DestroyItems()
		{
			m_set.DestroyItems();
		}

		public bool Contains(GameObject item)
		{
			return m_set.Contains(item);
		}

		#endregion
	}

	#endregion

	#region Logic

	public delegate void SetLogicEventHandler(object sender, SetLogicEventArgs e);
	public delegate void SetLogicEventHandler<T>(object sender, SetLogicEventArgs<T> e)
		where T : Component;

	public class SetLogicEventArgs : System.EventArgs
	{
		#region Properties

		public GameObject item { get; private set; }

		#endregion

		#region Constructors

		public SetLogicEventArgs(GameObject item)
		{
			this.item = item;
		}

		#endregion
	}

	public class SetLogicEventArgs<T> : System.EventArgs
		where T : Component
	{
		#region Properties

		public T item { get; private set; }

		#endregion

		#region Constructors

		public SetLogicEventArgs(T item)
		{
			this.item = item;
		}

		#endregion
	}

	[System.Serializable]
	public class SetLogic : IEnumerable<GameObject>, ISet
	{
		#region Variables

		private HashSet<GameObject> m_items = new HashSet<GameObject>();

		#endregion

		#region Properties

		public int count { get { return m_items.Count; } }
		public GameObject[] items { get => m_items.ToArray(); }

		#endregion

		#region Events

		public event SetLogicEventHandler ItemAdded;
		public event SetLogicEventHandler ItemRemoved;

		#endregion

		#region Methods

		public void Add(GameObject item, bool autoRemoveOnDestroy = true)
		{
			if (Contains(item))
				return;

			m_items.Add(item);
			if (ItemAdded != null)
			{
				ItemAdded.Invoke(this, new SetLogicEventArgs(item));
			}

			if (autoRemoveOnDestroy)
			{
				PoolItem poolItem = item.GetComponent<PoolItem>();
				if (poolItem == null)
				{
					// When object is added, add DestroyListener to ensure item is removed from set when destroyed
					Destroy_UnityEvents destroyListener;
					item.TryAddComponent(out destroyListener);

					destroyListener.onDestroyed.AddListener(() =>
					{
						Remove(item);
					});
				}
				else
				{
					// When object is added, add EnableDisableListener to ensure item is removed from set when destroyed
					EnableDisable_UnityEvents enableDisableListener;
					item.TryAddComponent(out enableDisableListener);

					enableDisableListener.onDisabled.AddListener(RemovePoolItem);
				}
			}
		}

		private void RemovePoolItem(GameObject item)
		{
			var enableDisableListener = item.GetComponent<EnableDisable_UnityEvents>();
			if (enableDisableListener != null)
			{
				enableDisableListener.onDisabled.RemoveListener(RemovePoolItem);
			}

			Remove(item);
		}

		public void Remove(GameObject item)
		{
			if (!Contains(item))
				return;

			m_items.Remove(item);
			if (ItemRemoved != null)
			{
				ItemRemoved.Invoke(this, new SetLogicEventArgs(item));
			}
		}

		public void DestroyItems()
		{
			m_items.DestroyItems();
		}

		public bool Contains(GameObject item)
		{
			return m_items.Contains(item);
		}

		public IEnumerator GetEnumerator()
		{
			return m_items.GetEnumerator();
		}

		IEnumerator<GameObject> IEnumerable<GameObject>.GetEnumerator()
		{
			return m_items.GetEnumerator();
		}

		#endregion
	}

	[System.Serializable]
	public class SetLogic<T>
		where T : Component
	{
		#region Variables

		private HashSet<T> m_items = new HashSet<T>();

		#endregion

		#region Properties

		public int count { get { return m_items.Count; } }
		public T[] items { get => m_items.ToArray(); }

		#endregion

		#region Events

		public event SetLogicEventHandler<T> ItemAdded;
		public event SetLogicEventHandler<T> ItemRemoved;

		#endregion

		#region Methods

		public void Add(T item, bool autoRemoveOnDestroy = true)
		{
			if (Contains(item))
				return;

			m_items.Add(item);
			if (ItemAdded != null)
			{
				ItemAdded.Invoke(this, new SetLogicEventArgs<T>(item));
			}

			if (autoRemoveOnDestroy)
			{
				// When object is added, add DestroyListener to ensure item is removed from set when destroyed
				Destroy_UnityEvents destroyListener;
				item.gameObject.TryAddComponent(out destroyListener);

				destroyListener.onDestroyed.AddListener(() =>
				{
					Remove(item);
				});
			}
		}

		public void Remove(T item)
		{
			if (!Contains(item))
				return;

			m_items.Remove(item);
			if (ItemRemoved != null)
			{
				ItemRemoved.Invoke(this, new SetLogicEventArgs<T>(item));
			}
		}

		public bool Contains(T item)
		{
			return m_items.Contains(item);
		}

		#endregion
	}

	#endregion
}
