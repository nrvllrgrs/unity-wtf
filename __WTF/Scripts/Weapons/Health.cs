using Sirenix.OdinInspector;
using Ludiq;

namespace UnityEngine.Workshop
{
	public class Health : MonoBehaviour, IHealth
	{
		#region Variables

		/// <summary>
		/// Maximum health
		/// </summary>
		[SerializeField, Tooltip("Maximum health"), MinValue(0f), BoxGroup("Health Settings")]
		private float m_maxHealth = 100f;

		/// <summary>
		/// Current health
		/// </summary>
		[SerializeField, Tooltip("Current health"), MinValue(0f), BoxGroup("Health Settings")]
		private float m_health = 100f;

		/// <summary>
		/// Indicates whether gameObject is destroyed when health is zero
		/// </summary>
		[SerializeField, Tooltip("Indicates whether gameObject is destroyed when health is zero"), BoxGroup("Health Settings")]
		private bool m_destroyOnKilled;

		/// <summary>
		/// Seconds before regeneration begins
		/// </summary>
		[SerializeField, Tooltip("Seconds before regeneration begins"), BoxGroup("Regeneration Settings")]
		private float m_regenerationDelay;

		/// <summary>
		/// Health regenerated per second
		/// </summary>
		[Tooltip("Health regenerated per second"), BoxGroup("Regeneration Settings")]
		public float regenerationRate;

		/// <summary>
		/// Seconds of invulnerability after taking damage
		/// </summary>
		[SerializeField, Tooltip("Seconds of invulnerability after receiving damage"), BoxGroup("Invulnerability Settings")]
		private float m_invulnerabilityTime;

		private float m_lastDamagedTimestamp = Mathf.NegativeInfinity;

		/// <summary>
		/// Indicates whether damaged this frame
		/// </summary>
		private bool m_frameDamaged;

		#endregion

		#region Events

		public event System.EventHandler ValueChanged;
		public event HealthEventHandler Healing;
		public event HealthEventHandler Healed;
		public event HealthEventHandler Damaging;
		public event HealthEventHandler Damaged;
		public event HealthEventHandler Killed;

		#endregion

		#region Properties

		public bool isAlive { get { return value > 0; } }
		public float maxHealth { get { return m_maxHealth; } }

		public virtual float value
		{
			get { return m_health; }
			set
			{
				// No change occurred, skip
				if (value == this.value)
					return;

				m_health = value;

				if (ValueChanged != null)
				{
					ValueChanged.Invoke(this, System.EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Indicates whether component can regenerate at this moment
		/// </summary>
		public bool canRegenerate { get { return regenerationRate < 0f || Time.time > m_lastDamagedTimestamp + m_regenerationDelay; } }

		/// <summary>
		/// Indicates whether component is invulerable at this moment
		/// </summary>
		public bool isInvulnerable => !m_frameDamaged && Time.time < m_lastDamagedTimestamp + m_invulnerabilityTime;

		#endregion

		#region Methods

		public virtual void Heal(HealthEventArgs e)
		{
			float nextHealth = GetNextHealth(e.delta);

			// Check whether doing anything
			if (nextHealth == value)
				return;

			// Check whether actually damaging
			if (nextHealth < value)
			{
				Damage(e);
				return;
			}

			if (Healing != null)
			{
				Healing.Invoke(this, e);
			}

			// Change health
			value = nextHealth;

			if (Healed != null)
			{
				Healed.Invoke(this, e);
			}
		}

		public virtual void Damage(HealthEventArgs e)
		{
			float nextHealth = GetNextHealth(-e.delta);

			// Check whether doing anything
			if (nextHealth == value)
				return;

			// Check whether actually healing
			if (nextHealth > value)
			{
				Heal(e);
				return;
			}
		
			// Ignore incoming damage if invulnerable
			if (isInvulnerable)
				return;

			Damaging?.Invoke(this, e);

			// Change health
			value = nextHealth;

			//  Remember when last damaged
			m_lastDamagedTimestamp = Time.time;
			m_frameDamaged = true;

			Damaged?.Invoke(this, e);

			// Object has been killed
			if (value == 0f)
			{
				if (Killed != null)
				{
					Killed.Invoke(this, e);
				}
				
				if (m_destroyOnKilled)
				{
					GameObjectUtil.Destroy(gameObject);
				}
			}
		}

		public void Kill(GameObject killer = null)
		{
			Damage(new HealthEventArgs(gameObject, killer, value));
		}

		protected virtual void Update()
		{
			if (canRegenerate && regenerationRate != 0f)
			{
				Heal(new HealthEventArgs(gameObject, regenerationRate * Time.deltaTime));
			}
		}

		protected virtual void LateUpdate()
		{
			m_frameDamaged = false;
		}

		private float GetNextHealth(float delta)
		{
			return Mathf.Clamp(m_health + delta, 0, m_maxHealth);
		}

		#endregion
	}

	public delegate void HealthEventHandler(object sender, HealthEventArgs e);

	[System.Serializable, IncludeInSettings(true)]
	public class HealthEventArgs : System.EventArgs
	{
		#region Properties

		public GameObject victim { get; private set; }
		public GameObject killer { get; private set; }

		/// <summary>
		/// Change in health
		/// </summary>
		public float delta { get; private set; }

		public Vector3 origin { get; private set; }
		public Vector3 contact { get; private set; }
		public Vector3 normal { get; private set; }

		#endregion

		#region Constructors

		public HealthEventArgs(GameObject victim, float delta)
			: this(victim, null, delta)
		{ }

		public HealthEventArgs(GameObject victim, GameObject killer, float delta)
		{
			this.victim = victim;
			this.killer = killer;
			this.delta = delta;
		}

		public HealthEventArgs(GameObject victim, GameObject killer, float delta, Vector3 contact, Vector3 normal)
			: this(victim, killer, delta)
		{
			this.contact = contact;
			this.normal = normal;
		}

		#endregion
	}
}