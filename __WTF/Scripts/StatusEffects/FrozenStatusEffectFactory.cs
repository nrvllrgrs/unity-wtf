using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	[CreateAssetMenu(menuName = "Workshop/Status Effects/Frozen")]
	public class FrozenStatusEffectFactory : StatusEffectFactory<FrozenStatusEffectData, FrozenStatusEffect>
	{ }

	[System.Serializable]
	public class FrozenStatusEffectData : StatusEffectData
	{
		#region Variables

		[Header("Frozen Settings")]
		public float speedModifier = 0f;

		[ValueDropdown("GetDamageTypes")]
		public string counterDamageType;

		[ValueDropdown("GetDamageTypes")]
		public string shatterDamageType;

		#endregion

		#region Properties

		public override string key => "frozen";

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		private string[] GetDamageTypes()
		{
			return ArmorManager.AttemptGetDamageTypes();
		}

#endif
		#endregion
	}

	public class FrozenStatusEffect : StatusEffect<FrozenStatusEffectData>
	{
		#region Variables

		private Health m_health;

		#endregion

		#region Methods

		protected override void CustomApply()
		{
			m_health = target.GetComponentInParent<Health>();
			if (m_health != null)
			{
				m_health.Damaged += Damaged;
			}
		}

		protected override void CustomUnapply()
		{
			if (m_health != null)
			{
				m_health.Damaged -= Damaged;
			}
		}

		private void Damaged(object sender, HealthEventArgs e)
		{
			if ((e.impactDamage > 0f && e.impactDamageType == data.counterDamageType)
				|| (e.splashDamage > 0f && e.splashDamageType == data.counterDamageType))
			{
				Unapply();
			}

			if (e.impactDamage > 0f && !string.IsNullOrWhiteSpace(data.shatterDamageType) && e.impactDamageType == data.shatterDamageType)
			{
				Unapply();
				m_health.Kill(e.killer);
			}
		}

		#endregion
	}
}