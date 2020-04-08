using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class PoolItem : MonoBehaviour
	{
		#region Variables

		[SerializeField, ValueDropdown("GetKeys"), HideIf("useAutoKey")]
		private string m_key;

		[SerializeField, ShowIf("useAutoKey")]
		private string m_autoKey;

		[SerializeField, ShowIf("useAutoKey"), MinValue(1)]
		private int m_maxCount;

		[SerializeField]
		private bool m_useAutoKey;

		private bool m_dirty;

		#endregion

		#region Properties

		public string key => m_useAutoKey ? m_autoKey : m_key;
		public int maxCount => m_maxCount;
		public bool useAutoKey => m_useAutoKey;

		#endregion

		#region Methods

		private void Awake()
		{
			this.WaitUntil(
				() => PoolManager.Exists,
				() => PoolManager.Instance.Add(this));
		}

		private void OnEnable()
		{
			if (m_dirty)
			{
				this.WaitUntil(
				() => PoolManager.Exists && PoolManager.Instance.Contains(this),
				() =>
				{
					// Reset PoolItem components
					foreach (var poolItem in GetComponentsInChildren<IPoolItem>())
					{
						poolItem.Restart();
					}
				});
			}
		}

		private void OnDisable()
		{
			m_dirty = true;
		}

		private void OnDestroy()
		{
			if (PoolManager.Exists)
			{
				PoolManager.Instance.Remove(this);
			}
		}

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		private IEnumerable<string> GetKeys()
		{
			if (!PoolManager.Exists)
				return new string[] { };

			return PoolManager.Instance.keys;
		}

#endif
		#endregion
	}

	public interface IPoolItem
	{
		#region Methods

		void Restart();

		#endregion
	}
}