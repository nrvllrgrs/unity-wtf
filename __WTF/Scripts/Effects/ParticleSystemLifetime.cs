using System.Linq;
using UnityEngine;

namespace UnityEngine.Workshop
{
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleSystemLifetime : MonoBehaviour
	{
		#region Variables

		private bool m_hasPlayed;

		private ParticleSystem m_particleSystem;
		private ParticleSystem[] m_particleSystems;

		#endregion

		#region Properties

		public new ParticleSystem particleSystem
		{
			get
			{
				if (m_particleSystem == null)
				{
					m_particleSystem = GetComponent<ParticleSystem>();
				}
				return m_particleSystem;
			}
		}

		public ParticleSystem[] childParticleSystems
		{
			get
			{
				if (m_particleSystems == null)
				{
					m_particleSystems = GetComponentsInChildren<ParticleSystem>();
				}
				return m_particleSystems;
			}
		}

		#endregion

		#region Methods

		private void Update()
		{
			if (particleSystem.isPlaying)
			{
				m_hasPlayed = true;
			}
			else if (m_hasPlayed)
			{
				if (childParticleSystems.All(x => x.isStopped))
				{
					GameObjectUtil.Destroy(gameObject);
				}
			}
		}

		#endregion
	}
}