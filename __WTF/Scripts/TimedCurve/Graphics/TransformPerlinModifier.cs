using UnityEngine;
using UnityEngine.Workshop;
using Sirenix.OdinInspector;

namespace Groundling
{
	public class TransformPerlinModifier : TimedCurveModifier<Transform, TransformInfo>
	{
		#region Variables

		[SerializeField, BoxGroup("Perlin Settings")]
		public Space space = Space.World;

		[BoxGroup("Perlin Settings")]
		public bool updatePosition = true;

		[BoxGroup("Perlin Settings"), ShowIf("updatePosition")]
		public float positionScale = 1f;

		[BoxGroup("Perlin Settings")]
		public bool updateEulerAngles = true;

		[BoxGroup("Perlin Settings"), ShowIf("updateEulerAngles")]
		public float eulerAnglesScale = 1f;

		[BoxGroup("Perlin Settings")]
		public bool updateScale = true;

		[BoxGroup("Perlin Settings"), ShowIf("updateScale")]
		public float scaleScale = 1f;

		[SerializeField, BoxGroup("Perlin Settings")]
		private bool m_resetOnTimedCurveEnd = true;

		#endregion

		#region Methods

		protected override TransformInfo GetValue()
		{
			switch (space)
			{
				case Space.World:
					return new TransformInfo(
						modified.position,
						modified.eulerAngles,
						modified.lossyScale);

				case Space.Self:
					return new TransformInfo(
						modified.localPosition,
						modified.localEulerAngles,
						modified.localScale);
			}
			return null;
		}

		protected override void SetValue(TransformInfo value)
		{
			if (modified == null)
				return;

			switch (space)
			{
				case Space.World:
					modified.position = value.position;
					modified.eulerAngles = value.eulerAngles;
					modified.localScale = value.scale;

					if (modified.parent != null)
					{
						modified.localScale += -modified.parent.lossyScale + Vector3.one;
					}
					break;

				case Space.Self:
					modified.localPosition = value.position;
					modified.localEulerAngles = value.eulerAngles;
					modified.localScale = value.scale;
					break;
			}
		}

		protected override TransformInfo Lerp(float value)
		{
			var current = GetValue();

			Vector3 position = updatePosition ? source.position : current.position;
			Vector3 eulerAngles = updateEulerAngles ? source.eulerAngles : current.eulerAngles;
			Vector3 scale = updateScale ? source.scale : current.scale;

			if (!m_resetOnTimedCurveEnd || !Mathf.Approximately(timedCurve.timePercent, 1f))
			{
				if (updatePosition)
				{
					float timeScale = Time.time * positionScale;
					float xScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(timeScale, 0f), -1f, 1f);
					float yScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(0f, timeScale), -1f, 1f);
					float zScale = value * UnityUtil.Remap01(Mathf.PerlinNoise(timeScale, timeScale), -1f, 1f);
					position += GetOffset(target.position, xScale, yScale, zScale);
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

			return new TransformInfo(position, eulerAngles, scale);
		}

		private Vector3 GetOffset(Vector3 variance, float xScale, float yScale, float zScale)
		{
			return new Vector3(variance.x * xScale, variance.y * yScale, variance.z * zScale);
		}

		#endregion
	}
}
