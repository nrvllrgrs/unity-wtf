using UnityEngine;
using UnityEngine.Workshop;
using Sirenix.OdinInspector;

namespace Groundling
{
	public sealed class RectTransformPerlinModifier : TimedCurveModifier<RectTransform, RectTransformInfo>
	{
		#region Variables

		[BoxGroup("Perlin Settings")]
		public bool updateAnchoredPosition = true;

		[ShowIf("updateAnchoredPosition"), BoxGroup("Perlin Settings")]
		public float anchoredPositionScale = 1f;

		[BoxGroup("Perlin Settings")]
		public bool updateSizeDelta = true;

		[ShowIf("updateSizeDelta"), BoxGroup("Perlin Settings")]
		public float sizeDeltaScale = 1f;

		[BoxGroup("Perlin Settings")]
		public bool updateEulerAngles = true;

		[ShowIf("updateEulerAngles"), BoxGroup("Perlin Settings")]
		public float eulerAnglesScale = 1f;

		[BoxGroup("Perlin Settings")]
		public bool updateScale = true;

		[ShowIf("updateScale"), BoxGroup("Perlin Settings")]
		public float scaleScale = 1f;

		[SerializeField, BoxGroup("Perlin Settings")]
		private bool m_resetOnTimedCurveEnd = true;

		#endregion

		#region Methods

		protected override RectTransformInfo GetValue()
		{
			return new RectTransformInfo()
			{
				anchoredPosition = modified.anchoredPosition,
				sizeDelta = modified.sizeDelta,
				eulerAngles = modified.localEulerAngles,
				scale = modified.localScale
			};
		}

		protected override void SetValue(RectTransformInfo value)
		{
			modified.anchoredPosition = value.anchoredPosition;
			modified.sizeDelta = value.sizeDelta;
			modified.localEulerAngles = value.eulerAngles;
			modified.localScale = value.scale;
		}

		protected override RectTransformInfo Lerp(float value)
		{
			Vector2 anchoredPosition = source.anchoredPosition;
			Vector2 sizeDelta = source.sizeDelta;
			Vector3 eulerAngles = source.eulerAngles;
			Vector3 scale = source.scale;

			if (!m_resetOnTimedCurveEnd || !Mathf.Approximately(timedCurve.timePercent, 1f))
			{
				if (updateAnchoredPosition)
				{
					float timeScale = Time.time * anchoredPositionScale;
					float xScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(timeScale, 0f), -1f, 1f);
					float yScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(0f, timeScale), -1f, 1f);
					anchoredPosition += GetOffset(target.anchoredPosition, xScale, yScale);
				}

				if (updateSizeDelta)
				{
					float timeScale = Time.time * sizeDeltaScale;
					float xScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(timeScale, 0f), -1f, 1f);
					float yScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(0f, timeScale), -1f, 1f);
					anchoredPosition += GetOffset(target.sizeDelta, xScale, yScale);
				}

				if (updateEulerAngles)
				{
					float timeScale = Time.time * eulerAnglesScale;
					float xScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(timeScale, 0f), -1f, 1f);
					float yScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(0f, timeScale), -1f, 1f);
					float zScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(timeScale, timeScale), -1f, 1f);
					eulerAngles += GetOffset(target.eulerAngles, xScale, yScale, zScale);
				}

				if (updateScale)
				{
					float timeScale = Time.time * scaleScale;
					float xScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(timeScale, 0f), -1f, 1f);
					float yScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(0f, timeScale), -1f, 1f);
					float zScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(timeScale, timeScale), -1f, 1f);
					scale += GetOffset(target.scale, xScale, yScale, zScale);
				}
			}

			return new RectTransformInfo()
			{
				anchoredPosition = anchoredPosition,
				sizeDelta = sizeDelta,
				eulerAngles = eulerAngles,
				scale = scale
			};
		}

		private Vector2 GetOffset(Vector2 variance, float xScale, float yScale)
		{
			return new Vector2(variance.x * xScale, variance.y * yScale);
		}

		private Vector3 GetOffset(Vector3 variance, float xScale, float yScale, float zScale)
		{
			return new Vector3(variance.x * xScale, variance.y * yScale, variance.z * zScale);
		}

		#endregion
	}
}
