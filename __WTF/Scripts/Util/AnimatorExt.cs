using System.Collections.Generic;
using UnityEngine;
using Ludiq;

[IncludeInSettings(true)]
public static class AnimatorExt
{
	public static bool TryGetLayerWeight(this Animator animator, string layerName, out float weight)
	{
		if (!string.IsNullOrWhiteSpace(layerName))
		{
			if (animator.gameObject.TryAddComponent(out AnimatorLayerNameMap animatorLayerNameMap))
			{
				animatorLayerNameMap.hideFlags |= HideFlags.HideAndDontSave;
			}

			if (animatorLayerNameMap.TryGetLayerIndex(layerName, out int layerIndex))
			{
				weight = animator.GetLayerWeight(layerIndex);
				return true;
			}
		}

		weight = 0f;
		return false;
	}

	public static void SetLayerWeight(this Animator animator, string layerName, float weight)
	{
		if (string.IsNullOrWhiteSpace(layerName))
			return;

		if (animator.gameObject.TryAddComponent(out AnimatorLayerNameMap animatorLayerNameMap))
		{
			animatorLayerNameMap.hideFlags |= HideFlags.HideAndDontSave;
		}

		if (animatorLayerNameMap.TryGetLayerIndex(layerName, out int layerIndex))
		{
			animator.SetLayerWeight(layerIndex, weight);
		}
	}

	public static bool HasLayerWeight(this Animator animator, string layerName)
	{
		return animator.TryGetLayerWeight(layerName, out float weight);
	}

	public static bool HasParameter(this Animator animator, string parameterName)
	{
		foreach (var parameter in animator.parameters)
		{
			if (Equals(parameter.name, parameterName))
				return true;
		}

		return false;
	}

	[RequireComponent(typeof(Animator))]
	public class AnimatorLayerNameMap : MonoBehaviour
	{
		#region Variables

		private Animator m_animator;
		private Dictionary<string, int> m_map;

		#endregion

		#region Properties

		public Animator animator
		{
			get
			{
				if (m_animator == null)
				{
					m_animator = GetComponent<Animator>();
				}
				return m_animator;
			}
		}

		public Dictionary<string, int> map
		{
			get
			{
				if (m_map == null)
				{
					m_map = new Dictionary<string, int>();

					// Populate map with LUT
					for (int i = 0; i < animator.layerCount; ++i)
					{
						m_map.Add(animator.GetLayerName(i), i);
					}
				}
				return m_map;
			}
		}

		#endregion

		#region Methods

		public bool TryGetLayerIndex(string layerName, out int layerIndex)
		{
			if (map.ContainsKey(layerName))
			{
				layerIndex = m_map[layerName];
				return true;
			}

			layerIndex = -1;
			return false;
		}

		#endregion
	}
}