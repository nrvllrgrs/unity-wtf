using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class PlayRandomAudioClip : MonoBehaviour
	{
		#region Variables

		[SerializeField, InfoBox("Use Audio Source on this object.", "IsAudioUndefined")]
		private AudioSource m_audio;

		[SerializeField]
		private List<AudioClip> m_clips;

		[SerializeField]
		private bool m_playOnEnable = true, m_stopOnDisable = true;

		#endregion

		#region Properties

		public new AudioSource audio
		{
			get
			{
				if (m_audio == null)
				{
					m_audio = GetComponent<AudioSource>();

					if (m_audio == null)
					{
						throw new System.Exception(string.Format("Audio Source is undefined for object {0}!", name));
					}
				}
				return m_audio;
			}
		}

		#endregion

		#region Methods

		private void OnEnable()
		{
			if (m_playOnEnable)
			{
				Play();
			}
		}

		private void OnDisable()
		{
			if (m_stopOnDisable)
			{
				audio.Stop();
			}
		}

		public virtual void Play()
		{
			audio.clip = m_clips.Random();
			audio.Play();
		}

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		private bool IsAudioUndefined()
		{
			return m_audio == null;
		}

#endif
		#endregion
	}
}
