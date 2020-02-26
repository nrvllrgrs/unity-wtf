using System.Linq;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class AnimationCurveComposite : MonoBehaviour
	{
		#region Enumerators

		public enum BlendType
		{
			Additive,
			Multiply,
		}

		public enum JoinType
		{
			Both,
			Average,
			First,
			Last,
		}

		#endregion

		#region Variables

		[SerializeField]
		private AnimationCurve m_curve;

		[SerializeField]
		private BlendType m_blend = BlendType.Multiply;

		[SerializeField]
		private CurveInfo[] m_curves;

		public const float FUDGE_FACTOR = 0.001f;

		#endregion

		#region Properties

		public AnimationCurve curve
		{
			get
			{
				if (m_curve == null)
				{
					m_curve = new AnimationCurve();
					foreach (var keyComposite in GetKeyframeComposites())
					{
						m_curve.AddKey(new Keyframe(
							keyComposite.Key,
							Evaluate(keyComposite.Key),
							keyComposite.Average(x => x.inTangent),
							keyComposite.Average(x => x.outTangent),
							keyComposite.Average(x => x.inWeight),
							keyComposite.Average(x => x.outWeight)));
					}
				}
				return m_curve;
			}
		}

		#endregion

		#region Methods

		private IOrderedEnumerable<IGrouping<float, Keyframe>> GetKeyframeComposites()
		{
			// Collect all keys, order by time
			return from key in m_curves.SelectMany(x => x.curve.keys)
				   where key.time.Between(0f, 1f)
				   group key by key.time into keyComposite
				   orderby keyComposite.Key
				   select keyComposite;
		}

		public float Evaluate(float t)
		{
			t = Mathf.Clamp01(t);

			float value = 0f;
			switch (m_blend)
			{
				case BlendType.Additive:
					foreach (var info in m_curves)
					{
						value += info.curve.Evaluate(t);
					}
					break;

				case BlendType.Multiply:
					value = 1f;
					foreach (var info in m_curves)
					{
						value *= info.curve.Evaluate(t);
					}
					break;
			}

			return value;
		}

		[Button]
		public AnimationCurve Generate()
		{
			m_curve = null;
			return curve;
		}

		#endregion

		#region Structures

		[System.Serializable]
		public class CurveInfo
		{
			#region Variables

			[SerializeField, Required]
			private AnimationCurve m_rawCurve;

			[SerializeField]
			private float m_amplitudeOffset;

			[SerializeField, Range(-1f, 1f)]
			private float m_frequencyOffset;

			[SerializeField, MinValue(1)]
			private int m_repeat = 1;

			[SerializeField, ShowIf("IsRepeatGreaterThanOne")]
			private JoinType m_join = JoinType.Both;

			[SerializeField]
			private AnimationCurve m_curve;

			#endregion

			#region Properties

			public AnimationCurve curve
			{
				get
				{
					if (m_curve == null && m_rawCurve != null)
					{
						m_curve = new AnimationCurve();

						float count = m_repeat;
						float step = 1 / count;

						for (int i = 0; i < count; ++i)
						{
							for (int j = 0; j < m_rawCurve.length; ++j)
							{
								// First key of repeating curve has already been handled
								if (j == 0 && i > 0)
									continue;

								float time = (m_rawCurve.keys[j].time * step) + (i * step) + (m_frequencyOffset * step);

								float value = m_rawCurve.keys[j].value;
								float inTangent = m_rawCurve.keys[j].inTangent;
								float outTangent = m_rawCurve.keys[j].outTangent;
								float inWeight = m_rawCurve.keys[j].inWeight;
								float outWeight = m_rawCurve.keys[j].outWeight;

								// If final keyframe in repeating curve...
								if (i < count - 1 && j == m_rawCurve.length - 1)
								{
									var firstKey = m_rawCurve.keys[0];
									var lastKey = m_rawCurve.keys[j];

									switch (m_join)
									{
										case JoinType.Both:
											// Shift time backwards by a fudge
											// Add key from the last keyframe
											m_curve.AddKey(new Keyframe(time - (FUDGE_FACTOR * step), lastKey.value + m_amplitudeOffset, lastKey.inTangent, lastKey.outTangent, lastKey.inWeight, lastKey.outWeight));

											// Shift time forwards by a fudge
											time += FUDGE_FACTOR * step;

											// Setup values from the first keyframe 
											value = firstKey.value;
											inTangent = firstKey.inTangent;
											outTangent = firstKey.outTangent;
											inWeight = firstKey.inWeight;
											outWeight = firstKey.outWeight;
											break;

										case JoinType.Average:
											// Calculate average values
											value = (firstKey.value + lastKey.value) / 2f;
											inTangent = lastKey.inTangent;
											outTangent = firstKey.outTangent;
											inWeight = lastKey.outWeight;
											outWeight = firstKey.outWeight;
											break;

										case JoinType.First:
											value = firstKey.value;
											inTangent = firstKey.inTangent;
											outTangent = firstKey.outTangent;
											inWeight = firstKey.inWeight;
											outWeight = firstKey.outWeight;
											break;

										case JoinType.Last:
											value = lastKey.value;
											inTangent = lastKey.inTangent;
											outTangent = lastKey.outTangent;
											inWeight = lastKey.inWeight;
											outWeight = lastKey.outWeight;
											break;
									}
								}

								m_curve.AddKey(new Keyframe(time, value + m_amplitudeOffset, inTangent, outTangent, inWeight, outWeight));
							}
						}
					}
					return m_curve;
				}
			}

			#endregion

			#region Methods

			[Button]
			public AnimationCurve Generate()
			{
				m_curve = null;
				return curve;
			}

			#endregion

			#region Editor Methods
#if UNITY_EDITOR

			private bool IsRepeatGreaterThanOne()
			{
				return m_repeat > 1;
			}

#endif
			#endregion
		}

		#endregion
	}
}
