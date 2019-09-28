using System.Collections.Generic;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class AudioManager : Singleton<AudioManager>
	{
		#region Variables

		[SerializeField, Required]
		private AudioMixer m_mixer;

		private AudioListener m_listener;
		private Dictionary<string, TimedCurve> m_map = new Dictionary<string, TimedCurve>();

		#endregion

		#region Properties

		public AudioMixer mixer => m_mixer;

		public AudioListener listener
		{
			get
			{
				if (m_listener == null)
				{
					m_listener = FindObjectOfType<AudioListener>();
				}
				return m_listener;
			}
		}

		#endregion

		#region Methods

		private void Fade(string audioParam, float duration, bool forward)
		{
			TimedCurve timedCurve;
			if (!m_map.ContainsKey(audioParam))
			{
				timedCurve = gameObject.AddComponent<TimedCurve>();
				timedCurve.curve = AnimationCurve.Linear(0f, 1f, 1f, 0f);
				timedCurve.duration = duration;
				timedCurve.wrapMode = WrapMode.Default;

				// Add timed curve for future use
				m_map.Add(audioParam, timedCurve);

				// Start watching curve
				// Must use anonymous function so timed curve maps with audio parameter
				timedCurve.onValueChanged.AddListener((value) =>
				{
					// Remap 0 to 1 value; 0dB to -80dB (i.e. silent)
					mixer.SetFloat(audioParam, UnityUtil.Remap01(value, -80f, 0f));
				});
			}
			else
			{
				timedCurve = m_map[audioParam];
				timedCurve.duration = duration;
			}

			if (forward)
			{
				timedCurve.Forward();
			}
			else
			{
				timedCurve.Reverse();
			}
		}

		#endregion

		#region Static Methods

		public static void FadeOut(string audioParam, float duration)
		{
			Instance.Fade(audioParam, duration, true);
		}

		public static void FadeIn(string audioParam, float duration)
		{
			Instance.Fade(audioParam, duration, false);
		}

		#endregion
	}
}