using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public delegate void StatusEffectEvent(object sender, StatusEffectEventArgs e);

	public class StatusEffectEventArgs : System.EventArgs
	{
		#region Properties

		public string statusEffect { get; private set; }

		#endregion

		#region Constructors

		public StatusEffectEventArgs(string statusEffect)
		{
			this.statusEffect = statusEffect;
		}

		#endregion
	}

	public class StatusEffectControl : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private Health m_health;

		[SerializeField]
		private ControlInfo[] m_controlInfos;

		private Dictionary<string, StatusEffect> m_statusEffects = new Dictionary<string, StatusEffect>();

		#endregion

		#region Events

		public event StatusEffectEvent StatusEffectStarted;
		public event StatusEffectEvent StatusEffectStopped;

		#endregion

		#region Properties

		public Health health => this.GetComponent(ref m_health);

		#endregion

		#region Methods

		private void OnEnable()
		{
			health.Damaged += Damaged;
			health.Killed += Killed;
		}	

		private void OnDisable()
		{
			health.Damaged -= Damaged;
			health.Killed -= Killed;
		}

		private void Damaged(object sender, HealthEventArgs e)
		{
			// If impactDamageType is the same as splashDamageType AND impact did not apply status effect
			// Try apply status effect from splashDamage
			if (!ApplyStatusEffect(e.impactDamage, e.impactDamageType) || e.impactDamageType != e.splashDamageType)
			{
				ApplyStatusEffect(e.splashDamage, e.splashDamageType);
			}
		}

		private void Killed(object sender, HealthEventArgs e)
		{
			// Character is dead, turn off all status effects
			foreach (var pair in m_statusEffects)
			{
				pair.Value.Unapply();
			}
		}

		private bool ApplyStatusEffect(float damage, string damageType)
		{
			if (damage > 0f && ArmorManager.Instance.TryGetStatusEffect(damageType, out string statusEffectKey, out float percent))
			{
				if (percent > 0f && Random.Range(0f, 1f) < percent)
				{
					ApplyStatusEffect(statusEffectKey);
					return true;
				}
			}
			return false;
		}

		public void ApplyStatusEffect(string statusEffect)
		{
			if (!m_statusEffects.TryGetValue(statusEffect, out StatusEffect instance))
			{
				instance = StatusEffectManager.Instance.GetStatusEffect(statusEffect, gameObject);

				// No status effect exists, skip
				if (instance == null)
					return;

				// Store status effect for future use
				m_statusEffects.Add(statusEffect, instance);
			}

			var info = m_controlInfos.FirstOrDefault(x => Equals(x.statusEffect, statusEffect));

			// Status effect is undefined or has no duration, skip
			if (info == null || info.duration <= 0f)
				return;

			bool isRunning = instance.isRunning;
			instance.Apply(info.duration);

			if (!isRunning)
			{
				StatusEffectStarted?.Invoke(gameObject, new StatusEffectEventArgs(statusEffect));
			}
		}

		public void InvokeUnapplyStatusEffect(string statusEffect)
		{
			if (!string.IsNullOrWhiteSpace(statusEffect))
			{
				StatusEffectStopped?.Invoke(gameObject, new StatusEffectEventArgs(statusEffect));
			}
		}

		public bool IsStatusEffectRunning(string statusEffect)
		{
			return m_statusEffects.TryGetValue(statusEffect, out StatusEffect instance)
				? instance.isRunning
				: false;
		}

		#endregion

		#region Structures

		[System.Serializable]
		public class ControlInfo
		{
			#region Variables

			[SerializeField, ValueDropdown("GetStatusEffectNames")]
			private string m_statusEffect;

			[SerializeField, MinValue(0f)]
			private float m_duration;

			#endregion

			#region Properties

			public string statusEffect => m_statusEffect;
			public float duration => m_duration;

			#endregion

			#region Editor Methods
#if UNITY_EDITOR

			private string[] GetStatusEffectNames()
			{
				return StatusEffectManager.Ready
					? StatusEffectManager.Instance.GetStatusEffectNames()
					: new string[] { };
			}

#endif
			#endregion
		}

		#endregion
	}
}
