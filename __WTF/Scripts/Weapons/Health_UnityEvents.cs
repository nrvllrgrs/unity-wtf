using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	[System.Serializable]
	public class HealthEvent : UnityEvent<HealthEventArgs>
	{ }

	public class Health_UnityEvents : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private Health m_health;

		#endregion

		#region Events

		[FoldoutGroup("Events")]
		public UnityEvent onValueChanged = new UnityEvent();

		[FoldoutGroup("Events")]
		public HealthEvent onHealing = new HealthEvent();

		[FoldoutGroup("Events")]
		public HealthEvent onHealed = new HealthEvent();

		[FoldoutGroup("Events")]
		public HealthEvent onDamaging = new HealthEvent();

		[FoldoutGroup("Events")]
		public HealthEvent onDamaged = new HealthEvent();

		[FoldoutGroup("Events")]
		public HealthEvent onKilled = new HealthEvent();

		#endregion

		#region Properties

		public Health health
		{
			get
			{
				if (m_health == null)
				{
					m_health = GetComponent<Health>();
				}
				return m_health;
			}
		}

		#endregion

		#region Methods

		private void OnEnable()
		{
			health.ValueChanged += ValueChanged;
			health.Healing += Healing;
			health.Healed += Healed;
			health.Damaging += Damaging;
			health.Damaged += Damaged;
			health.Killed += Killed;
		}

		private void OnDisable()
		{
			health.ValueChanged -= ValueChanged;
			health.Healing -= Healing;
			health.Healed -= Healed;
			health.Damaging -= Damaging;
			health.Damaged -= Damaged;
			health.Killed -= Killed;
		}

		private void ValueChanged(object sender, System.EventArgs e)
		{
			onValueChanged.Invoke();
		}

		private void Healing(object sender, HealthEventArgs e)
		{
			onHealing.Invoke(e);
		}

		private void Healed(object sender, HealthEventArgs e)
		{
			onHealed.Invoke(e);
		}

		private void Damaging(object sender, HealthEventArgs e)
		{
			onDamaging.Invoke(e);
		}

		private void Damaged(object sender, HealthEventArgs e)
		{
			onDamaged.Invoke(e);
		}

		private void Killed(object sender, HealthEventArgs e)
		{
			onKilled.Invoke(e);
		}

		#endregion
	}
}