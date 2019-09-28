namespace UnityEngine.Workshop
{
	public interface IHealth
	{
		GameObject gameObject { get; }

		void Heal(HealthEventArgs e);
		void Damage(HealthEventArgs e);
	}
}
