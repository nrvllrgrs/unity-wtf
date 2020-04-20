namespace UnityEngine.Workshop
{
	public interface IHealth
	{
		event System.EventHandler ValueChanged;
		event HealthEventHandler Healing;
		event HealthEventHandler Healed;
		event HealthEventHandler Hit;
		event HealthEventHandler Damaging;
		event HealthEventHandler Damaged;
		event HealthEventHandler Killed;

		GameObject gameObject { get; }
		float value { get; }
		float maxValue { get; }

		void Heal(HealthEventArgs e);
		void Damage(HealthEventArgs e);
	}
}
