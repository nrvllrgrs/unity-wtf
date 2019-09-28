using UnityEngine;
using UnityEngine.Workshop;
using Sirenix.OdinInspector;

namespace Groundling
{
	public abstract class TimedCurveMaterialModifer<T> : TimedCurveModifier<Renderer, T>
	{
		#region Variables

		[SerializeField]
		protected string propertyName;

		[MinValue(0), ShowIf("hasManyMaterials")]
		public int materialIndex = 0;

		#endregion

		#region Properties

		public Material material
		{
			get
			{
				return !hasManyMaterials
					? modified.material
					: modified.materials[materialIndex];
			}
		}

		public bool hasManyMaterials
		{
			get { return modified.sharedMaterials.Length > 1; }
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

		#endregion

	}
}