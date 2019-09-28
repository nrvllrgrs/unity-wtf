namespace UnityEngine.Workshop
{
	[RequireComponent(typeof(AudioSource))]
	public class AudioProgressControl : MonoBehaviour
	{
		#region Variables

		public bool restartOnUnpause;

		private AudioSource m_audio;
		private int m_playbackProgress;

		#endregion

		#region Properties

		public new AudioSource audio => this.GetComponent(ref m_audio);

		#endregion

		#region Methods

		public void Play(AudioClip clip)
		{
			// Stop previous voice
			Stop();

			if (clip != null)
			{
				// Start clip
				audio.clip = clip;
				audio.Play();
			}
		}

		public void Stop()
		{
			audio.Stop();
		}

		public void Pause()
		{
			m_playbackProgress = audio.timeSamples;
			audio.Pause();
		}

		public void UnPause()
		{
			if (!restartOnUnpause)
			{
				audio.timeSamples = m_playbackProgress;
			}

			audio.Play();
		}

		#endregion
	}
}