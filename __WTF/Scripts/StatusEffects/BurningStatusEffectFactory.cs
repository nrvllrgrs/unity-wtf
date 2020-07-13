namespace UnityEngine.Workshop
{
	[CreateAssetMenu(menuName = "Workshop/Status Effects/Burning")]
	public class BurningStatusEffectFactory : StatusEffectFactory<BurningStatusEffectData, BurningStatusEffect>
	{ }

	[System.Serializable]
	public class BurningStatusEffectData : StatusEffectData
	{
		#region Variables

		[Header("Burning Settings")]
		public float degenerationRate = 8f;

		#endregion

		#region Properties

		public override string key => "burning";

		#endregion
	}

	public class BurningStatusEffect : StatusEffect<BurningStatusEffectData>
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
				m_health.regenerationRate -= data.degenerationRate;
			}
		}

		protected override void CustomUnapply()
		{
			if (m_health != null)
			{
				m_health.regenerationRate += data.degenerationRate;
			}
		}

		#endregion
	}
}