using UnityEngine;
using UnityEngine.Workshop;
using Sirenix.OdinInspector;

namespace Groundling
{
	public class TransformModifier : TimedCurveModifier<Transform, TransformInfo>
	{
		#region Variables

		[SerializeField, BoxGroup("Transform Settings")]
		public Space space = Space.World;

		[BoxGroup("Transform Settings")]
		public bool updatePosition = true;

		[BoxGroup("Transform Settings")]
		public bool updateRotation = true;

		[BoxGroup("Transform Settings")]
		public bool updateScale = true;

		[SerializeField, BoxGroup("Transform Settings")]
		protected bool m_relativeToSource = false;

		[SerializeField, BoxGroup("Transform Settings"), ShowIf("m_relativeToSource")]
		protected bool m_setSourceOnTimedCurveEnd = false;

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
			if (!m_relativeToSource)
			{
				return new TransformInfo(
					updatePosition ? Vector3.Lerp(source.position, target.position, value) : source.position,
					updateRotation ? Vector3.Lerp(source.eulerAngles, target.eulerAngles, value) : source.eulerAngles,
					updateScale ? Vector3.Lerp(source.scale, target.scale, value) : source.scale);
			}
			else
			{
				var transformInfo  = new TransformInfo(
					updatePosition ? Vector3.Lerp(source.position, source.position + target.position, value) : source.position,
					updateRotation ? Vector3.Lerp(source.eulerAngles, source.eulerAngles + target.eulerAngles, value) : source.eulerAngles,
					updateScale ? Vector3.Lerp(source.scale, source.scale + target.scale, value) : source.scale);

				if (m_setSourceOnTimedCurveEnd && Mathf.Approximately(timedCurve.timePercent, 1f))
				{
					source = transformInfo;
				}
				return transformInfo;
			}
		}

		#endregion
	}

	[System.Serializable, Ludiq.IncludeInSettings(true)]
	public class TransformInfo
	{
		#region Variables

		[SerializeField]
		private Vector3 m_position;

		[SerializeField, LabelText("Rotation")]
		private Vector3 m_eulerAngles;

		[SerializeField]
		private Vector3 m_scale = Vector3.one;

		#endregion

		#region Properties

		public Vector3 position { get => m_position; set => m_position = value; }
		public Vector3 eulerAngles { get => m_eulerAngles; set => m_eulerAngles = value; }
		public Quaternion rotation { get => Quaternion.Euler(m_eulerAngles); set => m_eulerAngles = value.eulerAngles; }
		public Vector3 scale { get => m_scale; set => m_scale = value; }

		#endregion

		#region Constructors

		public TransformInfo()
			: this(Vector3.zero, Vector3.zero, Vector3.one)
		{ }

		public TransformInfo(Transform transform)
			: this(transform.position, transform.rotation, transform.lossyScale)
		{ }

		public TransformInfo(Vector3 position, Vector3 eulerAngles, Vector3 scale)
			: this(position, Quaternion.Euler(eulerAngles), scale)
		{ }

		public TransformInfo(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}

		#endregion

		#region Methods

		public bool Equals(Vector3 position, Vector3 eulerAngles)
		{
			return Equals(position, eulerAngles, Vector3.one);
		}

		public bool Equals(Vector3 position, Vector3 eulerAngles, Vector3 scale)
		{
			return object.Equals(this.position, position)
				&& object.Equals(this.eulerAngles, eulerAngles)
				&& object.Equals(this.scale, scale);
		}

		#endregion
	}
}