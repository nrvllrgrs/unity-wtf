namespace UnityEngine.Workshop
{
	[RequireComponent(typeof(AudioSource))]
	public class AudioLifetime : MonoBehaviour
	{
		#region Variables

		private AudioSource m_audio;
		private bool m_prevIsPlaying;

		#endregion

		#region Properties

		public new AudioSource audio
		{
			get
			{
				if (m_audio == null)
				{
					m_audio = GetComponent<AudioSource>();
				}
				return m_audio;
			}
		}

		#endregion

		#region Methods

		private void Update()
		{
			if (!audio.isPlaying && m_prevIsPlaying)
			{
				Destroy(gameObject);
			}

			m_prevIsPlaying = audio.isPlaying;
		}

		#endregion
	}
}