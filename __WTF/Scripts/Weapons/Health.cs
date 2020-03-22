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

		[SerializeField, ShowIf("m_destroyOnKilled"), Min(0), Tooltip("How long to wait until destroying the gameobject"), BoxGroup("Health Settings")]
		private float m_destroyDelay = 0f;

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
		/// Seconds between regeneration events
		/// </summary>
		[SerializeField, Tooltip("Seconds between regeneration events"), BoxGroup("Regeneration Settings"), MinValue(0f)]
		private float m_regenerationStep;

		[SerializeField, BoxGroup("Regeneration Settings")]
		private Transform m_regenerationContactPoint;

		[SerializeField, BoxGroup("Invulnerability Settings")]
		private bool m_isInvulnerable;

		/// <summary>
		/// Seconds of invulnerability after taking damage
		/// </summary>
		[SerializeField, Tooltip("Seconds of invulnerability after receiving damage"), BoxGroup("Invulnerability Settings")]
		private float m_invulnerabilityTime;

		private float m_lastDamagedTimestamp = Mathf.NegativeInfinity;
		private float m_lastRegenerationTimestamp = Mathf.NegativeInfinity;

		/// <summary>
		/// Indicates whether damaged this frame
		/// </summary>
		private bool m_frameDamaged;
		private bool m_dying = false;

		#endregion

		#region Events

		public event System.EventHandler ValueChanged;
		public event HealthEventHandler Healing;
		public event HealthEventHandler Healed;
		public event HealthEventHandler Hit;
		public event HealthEventHandler Damaging;
		public event HealthEventHandler Damaged;
		public event HealthEventHandler Killed;

		#endregion

		#region Properties

		public bool isAlive => value > 0;
		public float maxHealth { get => m_maxHealth; set => m_maxHealth = value; }

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
		public bool canRegenerate => (regenerationRate < 0f || Time.time >= m_lastDamagedTimestamp + m_regenerationDelay) && Time.time >= m_lastRegenerationTimestamp + m_regenerationStep;

		/// <summary>
		/// Transform to act as contact point during regeneration
		/// </summary>
		public Transform regenerationContactPoint
		{
			get
			{
				if (m_regenerationContactPoint == null)
				{
					m_regenerationContactPoint = transform;
				}
				return m_regenerationContactPoint;
			}
		}

		/// <summary>
		/// Indicates whether component is invulerable at this moment
		/// </summary>
		public bool isInvulnerable
		{
			get { return m_isInvulnerable || (!m_frameDamaged && Time.time < m_lastDamagedTimestamp + m_invulnerabilityTime); }
			set
			{
				if (isInvulnerable == value)
					return;

				m_isInvulnerable = value;
			}
		}

		#endregion

		#region Methods

		[Button, FoldoutGroup("Editor")]
		public virtual void Heal(HealthEventArgs e)
		{
			float nextHealth = GetNextHealth(e.delta);

			if (m_dying)
			{
				return;
			}

			// Check whether actually damaging
			if (nextHealth < value)
			{
				Damage(e);
				return;
			}

			// Check whether doing anything
			if (nextHealth == value)
				return;

			//Debug.LogFormat("{0} healing {1} for {2} points", e.killer.name, e.victim.name, e.impactDamage);

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

		[Button, FoldoutGroup("Editor")]
		public virtual void Damage(HealthEventArgs e)
		{
			float nextHealth = GetNextHealth(e.delta);

			// Check whether actually healing
			if (nextHealth > value)
			{
				Heal(e);
				return;
			}

			// Notify listeners that gameObject was hit (even though may not be damaged)
			Hit?.Invoke(this, e);

			if (m_dying)
			{
				return;
			}

			// Check whether doing anything
			if (nextHealth == value)
				return;

			//Debug.LogFormat("{0} damaging {1} for {2} {3} impact damage", e.killer.name, e.victim.name, e.impactDamage, e.impactDamageType);
		
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
				Killed?.Invoke(this, e);
				
				if (m_destroyOnKilled && !m_dying)
				{
					GameObjectUtil.Destroy(gameObject, m_destroyDelay);
					m_dying = true;
				}
			}
		}

		[Button, FoldoutGroup("Editor")]
		public void Kill(GameObject killer = null)
		{
			Damage(new HealthEventArgs(gameObject, killer, value, null));
		}

		protected virtual void Update()
		{
			if (canRegenerate && regenerationRate != 0f)
			{
				Heal(new HealthEventArgs(gameObject, -regenerationRate * (m_regenerationStep == 0f ? Time.deltaTime : m_regenerationStep), null, regenerationContactPoint.position, Vector3.zero));
				m_lastRegenerationTimestamp = Time.time;
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
		public float impactDamage { get; private set; }
		public string impactDamageType { get; private set; }

		public float splashDamage { get; private set; }
		public string splashDamageType { get; private set; }

		public Vector3 origin { get; private set; }
		public Vector3 contact { get; private set; }
		public Vector3 normal { get; private set; }

		/// <summary>
		/// Total health change
		/// </summary>
		public float delta => -(impactDamage + splashDamage);

		#endregion

		#region Constructors

		public HealthEventArgs(GameObject victim, float impactDamage, string impactDamageType)
			: this(victim, null, impactDamage, impactDamageType)
		{ }

		public HealthEventArgs(GameObject victim, float impactDamage, string impactDamageType, Vector3 contact, Vector3 normal)
			: this(victim, null, impactDamage, impactDamageType, contact, normal)
		{ }

		public HealthEventArgs(GameObject victim, float impactDamage, string impactDamageType, float splashDamage, string splashDamageType)
			: this(victim, null, impactDamage, impactDamageType, 0f, null)
		{ }

		public HealthEventArgs(GameObject victim, GameObject killer, float impactDamage, string impactDamageType)
			: this(victim, killer, impactDamage, impactDamageType, 0f, null)
		{ }

		public HealthEventArgs(GameObject victim, GameObject killer, float impactDamage, string impactDamageType, float splashDamage, string splashDamageType)
			: this(victim, killer, impactDamage, impactDamageType, splashDamage, splashDamageType, Vector3.zero, Vector3.zero)
		{ }

		public HealthEventArgs(GameObject victim, GameObject killer, float impactDamage, string impactDamageType, Vector3 contact, Vector3 normal)
			: this(victim, killer, impactDamage, impactDamageType, 0f, null, contact, normal)
		{ }

		public HealthEventArgs(GameObject victim, GameObject killer, float impactDamage, string impactDamageType, float splashDamage, string splashDamageType, Vector3 contact, Vector3 normal)
		{
			this.victim = victim;
			this.killer = killer;

			this.impactDamage = impactDamage;
			this.impactDamageType = impactDamageType;
			this.splashDamage = splashDamage;
			this.splashDamageType = splashDamageType;

			this.contact = contact;
			this.normal = normal;
		}

		#endregion

		#region Methods

		public bool IsHitDamageType(string damageType)
		{
			if (string.IsNullOrWhiteSpace(damageType))
				return false;

			return (impactDamage > 0f && Equals(damageType, impactDamageType))
				|| (splashDamage > 0f && Equals(damageType, splashDamageType));
		}

		#endregion
	}
}