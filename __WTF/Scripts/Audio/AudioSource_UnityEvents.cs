using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class AudioSource_UnityEvents : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private AudioSource m_audio;

		private bool m_wasPlaying;

		#endregion

		#region Events

		[FoldoutGroup("Events")]
		public UnityEvent onStarted = new UnityEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onStopped = new UnityEvent();

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
					onStarted.Invoke();
				}

				m_wasPlaying = true;
			}
			else
			{
				if (m_wasPlaying)
				{
					onStopped.Invoke();
				}

				m_wasPlaying = false;
			}
		}

		#endregion
	}
}
