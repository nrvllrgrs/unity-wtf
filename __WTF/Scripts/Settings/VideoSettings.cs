using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class VideoSettings : Singleton<VideoSettings>
	{
		#region Variables

		[SerializeField, LabelText("Full Screen")]
		private Toggle m_chkFullScreen;

		[SerializeField, LabelText("Resolutions")]
		private TMP_Dropdown m_cmbResolutions;

		[SerializeField, LabelText("Texture Quality")]
		private TMP_Dropdown m_cmbTexutreQuality;

		private Resolution[] m_resolutions;

		private static readonly string SETTINGS_FILENAME = "settings";

		#endregion

		#region Properties

		public Resolution[] resolutions
		{
			get
			{
				if (m_resolutions == null)
				{
					m_resolutions = Screen.resolutions;
				}
				return m_resolutions;
			}
		}

		#endregion

		#region Methods

		private void OnEnable()
		{
			if (m_chkFullScreen != null)
			{
				m_chkFullScreen.onValueChanged.AddListener(FullScreenChanged);
				m_chkFullScreen.isOn = Get("fullScreen", true);
			}

			if (m_cmbResolutions != null)
			{
				m_cmbResolutions.ClearOptions();

				List<string> resolutionOptions = new List<string>();
				int currResolutionIndex = 0;

				for (int i = 0; i < resolutions.Length; ++i)
				{
					int width = resolutions[i].width;
					int height = resolutions[i].height;

					resolutionOptions.Add(string.Format("{0} x {1}", width, height));

					if (Screen.currentResolution.width == width && Screen.currentResolution.height == height)
					{
						currResolutionIndex = i;
					}
				}

				m_cmbResolutions.AddOptions(resolutionOptions);
				m_cmbResolutions.value = currResolutionIndex;
				m_cmbResolutions.RefreshShownValue();

				m_cmbResolutions.onValueChanged.AddListener(ResolutionChanged);
				m_cmbResolutions.value = Get("resolution", currResolutionIndex);
			}
		}

		private void OnDisable()
		{
			if (m_chkFullScreen != null)
			{
				m_chkFullScreen.onValueChanged.RemoveListener(FullScreenChanged);
			}

			if (m_cmbResolutions != null)
			{
				m_cmbResolutions.onValueChanged.RemoveListener(ResolutionChanged);
			}
		}

		private void FullScreenChanged(bool isOn)
		{
			Screen.fullScreen = isOn;
			Set("fullScreen", isOn);
		}

		private void ResolutionChanged(int value)
		{
			Resolution resolution = resolutions[value];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
			Set("resolution", value);
		}

		private T Get<T>(string key, T fallback)
		{
			return PersistentData.Get(SETTINGS_FILENAME, string.Format("video:{0}", key), fallback);
		}

		private void Set<T>(string key, T value)
		{
			PersistentData.Set(SETTINGS_FILENAME, string.Format("video:{0}", key), value);
		}

		#endregion
	}
}
