using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RendererControl : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private bool m_initializeShow = true;

	private Dictionary<Renderer, bool> m_rendererStatuses;

	[SerializeField, ReadOnly]
	private int m_showCount = 0;

	#endregion

	#region Properties

	[ShowInInspector, ReadOnly]
	public bool showing
	{
		get
		{
			if (m_initializeShow)
			{
				return m_showCount <= 0;
			}
			else
			{
				return m_showCount > 0;
			}
		}
	}

	private int showCount
	{
		get { return m_showCount; }
		set
		{
			m_showCount = Mathf.Max(0, value);
			Show(showing);
		}
	}

	private Dictionary<Renderer, bool> rendererStatuses
	{
		get
		{
			if (m_rendererStatuses == null)
			{
				m_rendererStatuses = new Dictionary<Renderer, bool>();
				foreach (var renderer in GetChildRenderers(transform))
				{
					m_rendererStatuses.Add(renderer, false);
				}
			}
			return m_rendererStatuses;
		}
	}

	#endregion

	#region Methods
	
	[FoldoutGroup("Editor"), Button]
	public void Show()
	{
		showCount += m_initializeShow ? -1 : 1;
	}

	[FoldoutGroup("Editor"), Button]
	public void ForceShow()
	{
		if (m_initializeShow)
		{
			showCount = 0;
		}
		else if (!showing)
		{
			++showCount;
		}
	}

	[FoldoutGroup("Editor"), Button]
	public void Hide()
	{
		showCount += m_initializeShow ? 1 : -1;
	}

	[FoldoutGroup("Editor"), Button]
	public void ForceHide()
	{
		if (!m_initializeShow)
		{
			showCount = 0;
		}
		else if (showing)
		{
			++showCount;
		}
	}

	private void Show(bool isShown)
	{
		if (isShown)
		{
			foreach (var pair in rendererStatuses)
			{
				// Restore renderer on / off status
				pair.Key.enabled = pair.Value;
			}
		}
		else
		{
			var keys = new List<Renderer>(rendererStatuses.Keys);
			foreach (var renderer in keys)
			{
				if (renderer.enabled)
				{
					// Store whether renderer is on / off
					rendererStatuses[renderer] = renderer.enabled;
				}

				// Turn off renderer
				renderer.enabled = false;
			}
		}
	}

	private IEnumerable<Renderer> GetChildRenderers(Transform transform)
	{
		List<Renderer> items = new List<Renderer>();
		for (int i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i);
			var rendererControl = child.GetComponent<RendererControl>();

			// If renderer control exists, skip
			if (rendererControl != null)
				continue;

			var renderer = child.GetComponent<Renderer>();
			if (renderer != null)
			{
				items.Add(renderer);
			}

			items.AddRange(GetChildRenderers(child));
		}

		return items;
	}

	#endregion
}