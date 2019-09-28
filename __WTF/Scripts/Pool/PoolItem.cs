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

		#endregion

		#region Properties

		public string key { get { return m_useAutoKey ? m_autoKey : m_key; } }
		public int maxCount { get => m_maxCount; }
		public bool useAutoKey { get => m_useAutoKey; }

		#endregion

		#region Methods

		private void Start()
		{
			this.WaitUntil(
				() => { return PoolManager.Exists; },
				() => { PoolManager.Instance.Add(this); });
		}

		private void OnEnable()
		{
			var particleSystem = GetComponentInChildren<ParticleSystem>();
			if (particleSystem != null && particleSystem.main.playOnAwake)
			{
				particleSystem.Play();
			}

			foreach (var health in GetComponentsInChildren<Health>())
			{
				health.value = health.maxHealth;
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
}