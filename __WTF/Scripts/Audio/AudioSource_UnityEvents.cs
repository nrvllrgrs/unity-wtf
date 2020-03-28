using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	[System.Serializable]
	public class AudioSourceEvent : UnityEvent<AudioSource>
	{ }

	public class AudioSource_UnityEvents : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private AudioSource m_audio;

		private bool m_wasPlaying;

		#endregion

		#region Events

		[FoldoutGroup("Events")]
		public AudioSourceEvent onStarted = new AudioSourceEvent();

		[FoldoutGroup("Events")]
		public AudioSourceEvent onStopped = new AudioSourceEvent();

		#endregion

		#region Properties

		public new AudioSource audio => this.GetComponent(ref m_audio);

		#endregion

		#region Methods

		private void Update()
		{
			if (audio.isPlaying)
			{
				if (!m_wasPlaying)
				{
					onStarted.Invoke(audio);
				}

				m_wasPlaying = true;
			}
			else
			{
				if (m_wasPlaying)
				{
					onStopped.Invoke(audio);
				}

				m_wasPlaying = false;
			}
		}

		#endregion
	}
}
