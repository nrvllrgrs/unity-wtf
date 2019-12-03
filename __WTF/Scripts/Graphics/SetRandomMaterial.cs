using System.Linq;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class SetRandomMaterial : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private Renderer[] m_renderers;

		[SerializeField]
		private MaterialInfo[] m_materials;

		[SerializeField]
		private bool m_setOnAwake;

		#endregion

		#region Methods

		private void Start()
		{
			if (m_setOnAwake)
			{
				SetMaterial();
			}
		}

		public void SetMaterial()
		{
			if (!m_renderers.Any())
			{
				var renderer = GetComponent<Renderer>();
				if (renderer != null)
				{
					m_renderers = new[] { renderer };
				}
			}

			var material = m_materials.WeightedRandom();
			foreach (var renderer in m_renderers)
			{
				renderer.material = material;
			}
		}

		#endregion

		#region Structures

		[System.Serializable]
		public class MaterialInfo : IWeightedItem<Material>
		{
			#region Variables

			[SerializeField]
			private Material m_item;

			[SerializeField, MinValue(0f)]
			private float m_weight = 1f;

			#endregion

			#region Properties

			public Material item => m_item;
			public float weight => m_weight;

			#endregion
		}

		#endregion
	}
}
