using System.Linq;
using UnityEngine;
using UnityEngine.Workshop;
using Sirenix.OdinInspector;

namespace Groundling
{
	public abstract class TimedCurveMaterialModifer<T> : TimedCurveModifier<Renderer, T>
	{
		#region Variables

		[SerializeField, BoxGroup("Modifier Settings"), ShowIf("ShowModifiedList"), PropertyOrder(-1)]
		private Renderer[] m_renderers;

		[SerializeField, BoxGroup("Modifier Settings"), PropertyOrder(-1)]
		protected bool useList = false;

		[SerializeField, BoxGroup("Material Settings")]
		protected string propertyName;

		[MinValue(0), BoxGroup("Material Settings"), ShowIf("hasManyMaterials")]
		public int materialIndex = 0;

		private bool? m_hasManyMaterials = null;

		#endregion

		#region Properties

		protected Renderer[] renderers => gameObject.GetComponentsInChildren(ref m_renderers);

		public Material material
		{
			get
			{
				if (useList)
				{
					return materials.First();
				}

				return !hasManyMaterials
					? modified.material
					: modified.materials[materialIndex];
			}
		}

		public Material[] materials
		{
			get
			{
				if (!useList)
					return null;

				return !hasManyMaterials
					? renderers.Select(x => x.material).ToArray()
					: renderers.Select(x => x.materials[materialIndex]).ToArray();
			}
		}

		public bool hasManyMaterials
		{
			get
			{
				if (!m_hasManyMaterials.HasValue)
				{
					m_hasManyMaterials = !useList
						? modified.sharedMaterials.Length > 1
						: renderers.Any(x => x.sharedMaterials.Length > 1);
				}
				return m_hasManyMaterials.Value;
			}
		}

		#endregion

		#region Methods

		protected override void Awake()
		{
			if (!material.HasProperty(propertyName))
			{
				Debug.LogErrorFormat("{0} is undefined in material!", propertyName);
				Destroy(gameObject);
				return;
			}

			base.Awake();
		}

		protected override void SetValue(T value)
		{
			if (!useList)
			{
				SetValue(material, value);
			}
			else
			{
				foreach (var material in materials)
				{
					SetValue(material, value);
				}
			}
		}

		protected abstract void SetValue(Material material, T value);

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		protected override bool ShowModified() => !useList;
		protected virtual bool ShowModifiedList() => useList;

#endif
		#endregion
	}
}