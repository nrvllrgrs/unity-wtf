using UnityEngine.Workshop;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class MusicManager : Singleton<MusicManager>
	{
		#region Variables

		[SerializeField]
		private AudioSource m_audio;

		[SerializeField, HideIf("IsAudioSourceDefined")]
		private AudioClip m_music;

		[SerializeField, HideIf("IsAudioSourceDefined")]
		private AudioMixerGroup m_audioMixerGroup;

		[SerializeField]
		private bool m_playOnAwake = true;

		private AudioSource m_nextAudio;
		private TimedCurve m_timedCurve;
		private bool m_crossFade;

		#endregion

		#region Properties

		public new AudioSource audio
		{
			get { return m_audio; }
			private set { m_audio = value; }
		}

		public AudioSource nextAudio
		{
			get { return m_nextAudio; }
			private set { m_nextAudio = value; }
		}

		#endregion

		#region Methods

		protected override void Awake()
		{
			base.Awake();

			if (m_audio != null)
			{
				m_music = m_audio.clip;
				m_audioMixerGroup = m_audio.outputAudioMixerGroup;
			}
		}

		private void Start()
		{
			if (m_playOnAwake && m_music != null)
			{
				Play(m_music);
			}
		}

		public void Play(float volume = 1f)
		{
			if (m_music != null)
			{
				Play(m_music, volume);
			}
		}

		public void Play(AudioClip clip, float volume = 1f)
		{
			if (audio == null)
			{
				audio = InitAudioSource("SFX - Music", clip, volume);
			}
			else
			{
				audio.volume = volume;
				audio.clip = clip;
			}

			audio.Play();
		}

		public void Stop()
		{
			if (!audio.isPlaying)
			{
				Debug.LogError("Cannot fade out! No music is currently playing.");
				return;
			}

			audio.Stop();
		}

		public void FadeIn(AudioClip clip, float duration, AnimationCurve curve = null)
		{
			Play(clip, 0f);

			InitTimedCurve(duration, curve);
			m_timedCurve.StopAtEnd();

			// Play timed curve backwards to fade in main track
			m_crossFade = false;
			m_timedCurve.Reverse();
		}

		public void FadeOut(float duration, AnimationCurve curve = null)
		{
			if (!audio.isPlaying)
			{
				Debug.LogError("Cannot fade out! No music is currently playing.");
				return;
			}

			InitTimedCurve(duration, curve);

			m_crossFade = false;
			m_timedCurve.Play();
		}

		public void CrossFade()
		{
			if (nextAudio == null)
			{
				throw new System.Exception("Cannot cross fade without previous cross fade defined!");
			}

			// Use previous values to cross fade
			CrossFade(nextAudio.clip, m_timedCurve.duration, m_timedCurve.curve);
		}

		public void CrossFade(AudioClip nextClip, float duration, AnimationCurve curve = null)
		{
			InitTimedCurve(duration, curve);

			if (nextAudio == null)
			{
				nextAudio = InitAudioSource("SFX - Music", nextClip, 0f);
			}
			else
			{
				nextAudio.volume = 0f;
				nextAudio.clip = nextClip;
			}

			// Audio source may have been stopped from previous cross fade
			if (!nextAudio.isPlaying)
			{
				nextAudio.Play();
			}

			m_crossFade = true;
			m_timedCurve.Play();
		}

		private AudioSource InitAudioSource(string name, AudioClip clip, float volume)
		{
			var obj = new GameObject(name);
			obj.transform.SetParent(transform, false);

			AudioSource audio = obj.AddComponent<AudioSource>();
			audio.bypassReverbZones = true;
			audio.loop = true;
			audio.volume = volume;

			// 2D sound
			audio.spatialBlend = 0f;

			if (m_audioMixerGroup != null)
			{
				audio.outputAudioMixerGroup = m_audioMixerGroup;
			}

			audio.clip = clip;
			return audio;
		}

		#endregion

		#region Timed Curve Methods

		private void InitTimedCurve(float duration, AnimationCurve curve)
		{
			// Setup linear transition if curve undefined
			if (curve == null)
			{
				curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			}

			// Create timed curve to control cross fade
			if (m_timedCurve == null)
			{
				m_timedCurve = gameObject.AddComponent<TimedCurve>();
				m_timedCurve.wrapMode = WrapMode.Default;
				m_timedCurve.onValueChanged.AddListener(ValueChanged);
				m_timedCurve.onPlayStatusChanged.AddListener(PlayStatusChanged);
			}

			// Setup fade transition behavior
			m_timedCurve.duration = duration;
			m_timedCurve.curve = curve;
		}

		private void ValueChanged(float value)
		{
			audio.volume = 1f - value;
			if (m_crossFade)
			{
				nextAudio.volume = value;
			}
		}

		private void PlayStatusChanged(bool isPlaying)
		{
			if (!isPlaying)
			{
				if (!m_crossFade)
				{
					audio.Stop();
				}
				else
				{
					// Make next audio the new current audio
					UnityUtil.Swap(ref m_audio, ref m_nextAudio);

					// Stop previous audio
					nextAudio.Stop();
				}
			}
		}

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		private bool IsAudioSourceDefined()
		{
			return m_audio != null;
		}

#endif
		#endregion
	}
}
