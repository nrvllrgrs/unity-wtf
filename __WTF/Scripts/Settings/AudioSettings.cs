using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class AudioSettings : Singleton<AudioSettings>
	{
		#region Variables

		[SerializeField, Required]
		private AudioMixer m_audioMixer;

		[SerializeField]
		private List<VolumeSetting> m_volumeSettings = new List<VolumeSetting>();

		[SerializeField]
		private Toggle m_subtitles;

		private static readonly string SETTINGS_FILENAME = "settings";

		#endregion

		#region Properties

		public AudioMixer audioMixer { get => m_audioMixer; }
		public bool showSubtitles { get; private set; }

		#endregion

		#region Methods

		private void OnEnable()
		{
			foreach (var volumeSetting in m_volumeSettings)
			{
				volumeSetting.Enable();
			}

			if (m_subtitles != null)
			{
				m_subtitles.onValueChanged.AddListener(SubtitlesValueChanged);
				showSubtitles = PersistentData.Get(SETTINGS_FILENAME, "audio:subtitles", false);
			}
		}

		private void OnDisable()
		{
			foreach (var volumeSetting in m_volumeSettings)
			{
				volumeSetting.Disable();
			}

			if (m_subtitles != null)
			{
				m_subtitles.onValueChanged.RemoveListener(SubtitlesValueChanged);
			}
		}

		private void SubtitlesValueChanged(bool value)
		{
			PersistentData.Set(SETTINGS_FILENAME, "audio:subtitles", value);
			showSubtitles = value;
		}

		#endregion

		#region Structures

		[System.Serializable]
		public class VolumeSetting
		{
			#region Variables

			[SerializeField]
			private Slider m_slider;

			[SerializeField]
			private string m_audioParam;

			#endregion

			#region Methods

			public void Enable()
			{
				m_slider.onValueChanged.AddListener(ValueChanged);
				m_slider.value = PersistentData.Get(SETTINGS_FILENAME, string.Format("volume:{0}", m_audioParam), m_slider.maxValue);
			}

			public void Disable()
			{
				m_slider.onValueChanged.RemoveListener(ValueChanged);
			}

			private void ValueChanged(float value)
			{
				Instance.audioMixer.SetFloat(m_audioParam, UnityUtil.Remap(value, m_slider.minValue, m_slider.maxValue, -80f, 0f));
				PersistentData.Set(SETTINGS_FILENAME, string.Format("volume:{0}", m_audioParam), value);
			}

			#endregion
		}

		#endregion
	}
}