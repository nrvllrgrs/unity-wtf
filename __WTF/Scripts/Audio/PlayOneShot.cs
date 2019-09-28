using UnityEngine.Audio;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class PlayOneShot : MonoBehaviour
	{
		#region Variables

		[SerializeField, Required]
		private AudioClip m_clip;

		[SerializeField]
		private AudioMixerGroup m_audioMixerGroup;

		[SerializeField, Range(0f, 1f)]
		private float m_volume = 1f;

		[SerializeField, Range(0f, 1f), Tooltip("Sets how much the AudioSource is treated as a 3D source. 3D sources are affected by spatial position and spread.")]
		private float m_spatialBlend = 0f;

		[SerializeField, ShowIf("Is3DSource")]
		private Vector3 m_position, m_rotation;

		[SerializeField, ShowIf("Is3DSource")]
		private Space m_space = Space.Self;

		[SerializeField]
		private bool m_playOnAwake = true, m_dontDestroyOnLoad;

		#endregion

		#region Methods

		private void Start()
		{
			if (m_playOnAwake)
			{
				Play();
			}
		}

		public void Play()
		{
			if (m_spatialBlend > 0f)
			{
				Vector3 position;
				Quaternion rotation;

				if (m_space == Space.World)
				{
					position = m_position;
					rotation = Quaternion.Euler(m_rotation);
				}
				else
				{
					position = transform.position + m_position;
					rotation = Quaternion.Euler(transform.eulerAngles + m_rotation);
				}

				// 3D sound
				Play(m_clip, m_audioMixerGroup, m_volume, position, rotation, m_dontDestroyOnLoad);
			}
			else
			{
				// 2D sound
				Play(m_clip, m_audioMixerGroup, m_volume, m_dontDestroyOnLoad);
			}
		}

		public static void Play(AudioClip clip, AudioMixerGroup audioMixerGroup, float volume, bool dontDestroyOnLoad = false)
		{
			Play(clip, audioMixerGroup, volume, 0f, Vector3.zero, Quaternion.identity, dontDestroyOnLoad);
		}

		public static void Play(AudioClip clip, AudioMixerGroup audioMixerGroup, float volume, Vector3 position, Quaternion rotation, bool dontDestroyOnLoad = false)
		{
			Play(clip, audioMixerGroup, volume, 1f, position, rotation, dontDestroyOnLoad);
		}

		private static void Play(AudioClip clip, AudioMixerGroup audioMixerGroup, float volume, float spatialBlend, Vector3 position, Quaternion rotation, bool dontDestroyOnLoad)
		{
			if (clip == null)
				return;

			var obj = new GameObject("AudioSource - OneShot");
			obj.transform.SetPositionAndRotation(position, rotation);

			var audio = obj.AddComponent<AudioSource>();
			audio.clip = clip;
			audio.volume = volume;
			audio.spatialBlend = spatialBlend;

			if (audioMixerGroup != null)
			{
				audio.outputAudioMixerGroup = audioMixerGroup;
			}

			if (dontDestroyOnLoad)
			{
				obj.AddComponent<DontDestroyOnLoad>();
			}

			obj.AddComponent<AudioLifetime>();
			audio.Play();
		}

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		private bool Is3DSource()
		{
			return m_spatialBlend > 0f;
		}

#endif
		#endregion
	}
}