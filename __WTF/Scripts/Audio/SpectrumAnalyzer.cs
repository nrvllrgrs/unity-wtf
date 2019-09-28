using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Groundling
{
	public class SpectrumAnalyzer : MonoBehaviour
	{
		#region Variables

		public bool analyzeAudioListener = false;

		[HideIf("IsAudioListener")]
		public new AudioSource audio;

		[MinValue(6), MaxValue(13)]
		public uint samplesPower = 13;

		public int channel;
		public FFTWindow window;

		[SerializeField]
		private List<FrequencyBand> m_bands;

		[ReadOnly]
		public float[] samples;
		private int m_samplesCount = 0;

		private float m_avgBuffer;
		private float m_avgBufferModifier;

		private float m_maxBuffer;
		private float m_maxBufferModifier;

		private float m_totalBuffer;
		private float m_totalBufferModifier;

		#endregion

		#region Properties

		public FrequencyBand[] bands { get => m_bands.ToArray(); }
		public float avgValue { get => bands.Average(x => x.value); }
		public float maxValue { get => bands.Max(x => x.value); }
		public float totalValue { get => bands.Sum(x => x.value); }
		public float avgBuffer { get => m_avgBuffer; }
		public float maxBuffer { get => m_maxBuffer; }
		public float totalBuffer { get => m_totalBuffer; }

		private int samplesCount
		{
			get
			{
				if (m_samplesCount == 0)
				{
					m_samplesCount = (int)Mathf.Pow(2, samplesPower);
				}
				return m_samplesCount;
			}
		}

		#endregion

		#region Methods

		private void Start()
		{
			if (!analyzeAudioListener)
			{
				float minFrequency = bands.Min(x => x.minFrequency);
				float freqRange = bands.Max(x => x.maxFrequency) - minFrequency;

				int startIndex = 0;
				foreach (FrequencyBand band in bands)
				{
					int endIndex = (int)(((band.maxFrequency - minFrequency) - (band.minFrequency - minFrequency)) / freqRange * samplesCount);
					endIndex = Mathf.Max(endIndex + startIndex, 1);

					band.startIndex = startIndex;
					band.endIndex = Mathf.Min(endIndex, samplesCount - 1);

					startIndex = band.endIndex + 1;
				}
			}
		}

		private void Update()
		{
			samples = new float[samplesCount];

			if (analyzeAudioListener)
			{
				AudioListener.GetSpectrumData(samples, channel, window);
			}
			else
			{
				audio.GetSpectrumData(samples, channel, window);
			}

			UpdateBands();

			FrequencyBand.UpdateBuffer(bands.Average(x => x.value), ref m_avgBuffer, ref m_avgBufferModifier);
			FrequencyBand.UpdateBuffer(bands.Max(x => x.value), ref m_maxBuffer, ref m_maxBufferModifier);
			FrequencyBand.UpdateBuffer(bands.Sum(x => x.value), ref m_totalBuffer, ref m_totalBufferModifier);
		}

		private void UpdateBands()
		{
			foreach (FrequencyBand band in bands)
			{
				float value = 0f;
				for (int i = band.startIndex; i <= band.endIndex; ++i)
				{
					value += samples[i];
				}

				band.value = value / (band.endIndex - band.startIndex);
			}
		}

		#endregion

		#region Editor Methods

		private bool IsAudioListener()
		{
			return analyzeAudioListener;
		}

		[Button]
		private void SetupDefaultBands()
		{
			m_bands = new List<FrequencyBand>();
			m_bands.Add(new FrequencyBand()
			{
				name = "Sub Bass",
				minFrequency = 20,
				maxFrequency = 60,
			});
			m_bands.Add(new FrequencyBand()
			{
				name = "Bass",
				minFrequency = 60,
				maxFrequency = 250,
			});
			m_bands.Add(new FrequencyBand()
			{
				name = "Low Midrange",
				minFrequency = 250,
				maxFrequency = 500,
			});
			m_bands.Add(new FrequencyBand()
			{
				name = "Midrange",
				minFrequency = 500,
				maxFrequency = 2000,
			});
			m_bands.Add(new FrequencyBand()
			{
				name = "Upper Midrange",
				minFrequency = 2000,
				maxFrequency = 4000,
			});
			m_bands.Add(new FrequencyBand()
			{
				name = "Pressence",
				minFrequency = 4000,
				maxFrequency = 6000,
			});
			m_bands.Add(new FrequencyBand()
			{
				name = "Brilliance",
				minFrequency = 6000,
				maxFrequency = 20000,
			});
		}

		#endregion

		#region Structures

		[System.Serializable]
		public class FrequencyBand
		{
			#region Variables

			public string name;
			public float minFrequency;
			public float maxFrequency;

			[HideInInspector]
			public int startIndex, endIndex;

			[ReadOnly]
			public float value;

			private float m_buffer;
			private float m_bufferModifier;

			private const float INCREASE = 1.2f;
			private const float DECREASE = 0.005f;

			#endregion

			#region Properties

			[ShowInInspector]
			public float buffer
			{
				get
				{
					UpdateBuffer(value, ref m_buffer, ref m_bufferModifier);
					return m_buffer;
				}
			}

			#endregion

			#region Static Methods

			public static void UpdateBuffer(float value, ref float buffer, ref float bufferModifier)
			{
				if (value > buffer)
				{
					buffer = value;
					bufferModifier = DECREASE;
				}
				else if (value < buffer)
				{
					buffer -= bufferModifier;
					bufferModifier *= INCREASE;
				}
			}

			#endregion
		}

		#endregion
	}
}