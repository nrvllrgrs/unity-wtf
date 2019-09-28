using UnityEngine;
using UnityEngine.Workshop;
using Sirenix.OdinInspector;

namespace Groundling
{
	public class RectTransformModifier : TimedCurveModifier<RectTransform, RectTransformInfo>
	{
		#region Variables

		[BoxGroup("Rect Transform Settings")]
		public bool updateAnchoredPosition = true;

		[BoxGroup("Rect Transform Settings")]
		public bool updateSizeDelta = true;

		[BoxGroup("Rect Transform Settings")]
		public bool updateEulerAngles = true;

		[BoxGroup("Rect Transform Settings")]
		public bool updateScale = true;

		[SerializeField, BoxGroup("Rect Transform Settings")]
		protected bool m_relativeToSource = false;

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
			if (!m_relativeToSource)
			{
				return new RectTransformInfo()
				{
					anchoredPosition = updateAnchoredPosition ? Vector2.Lerp(source.anchoredPosition, target.anchoredPosition, value) : source.anchoredPosition,
					sizeDelta = updateSizeDelta ? Vector2.Lerp(source.sizeDelta, target.sizeDelta, value) : source.sizeDelta,
					eulerAngles = updateEulerAngles ? Vector3.Lerp(source.eulerAngles, target.eulerAngles, value) : source.eulerAngles,
					scale = updateScale ? Vector3.Lerp(source.scale, target.scale, value) : source.scale
				};
			}
			else
			{
				return new RectTransformInfo()
				{
					anchoredPosition = updateAnchoredPosition ? Vector2.Lerp(source.anchoredPosition, source.anchoredPosition + target.anchoredPosition, value) : source.anchoredPosition,
					sizeDelta = updateSizeDelta ? Vector2.Lerp(source.sizeDelta, source.sizeDelta + target.sizeDelta, value) : source.sizeDelta,
					eulerAngles = updateEulerAngles ? Vector3.Lerp(source.eulerAngles, source.eulerAngles + target.eulerAngles, value) : source.eulerAngles,
					scale = updateScale ? Vector3.Lerp(source.scale, source.scale + target.scale, value) : source.scale
				};
			}
		}

		#endregion
	}

	[System.Serializable]
	public class RectTransformInfo
	{
		public Vector2 anchoredPosition, sizeDelta;
		public Vector3 eulerAngles, scale;
	}
}