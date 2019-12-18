namespace UnityEngine.Workshop
{
	public class FaceCameraEventListener : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private Transform m_externalTransform;

		[SerializeField]
		private AxisType m_axisType = AxisType.Forward;

		[SerializeField, MinMax(0f, 90f, 0f, 180f)]
		private Vector2 m_angleLimits;

		private float m_percent;

		#endregion

		#region Events

		public event System.EventHandler FacingStarted;
		public event System.EventHandler FacingChanged;
		public event System.EventHandler FacingStopped;

		#endregion

		#region Properties

		public Transform facingTransform => m_externalTransform != null ? m_externalTransform : transform;

		public float percent
		{
			get => m_percent;
			private set
			{
				if (percent == value)
					return;

				// Facing moved into valid facing
				if (percent == 0 && value > 0)
				{
					FacingStarted?.Invoke(this, System.EventArgs.Empty);
				}
				// Facing moved into invalid facing
				else if (percent > 0 && value == 0)
				{
					FacingStopped?.Invoke(this, System.EventArgs.Empty);
				}
				else
				{
					FacingChanged?.Invoke(this, System.EventArgs.Empty);
				}

				m_percent = value;
			}
		}

		#endregion

		#region Methods

		private void Update()
		{
			if (Camera.main != null)
			{
				float angle = Vector3.Angle(facingTransform.GetAxis(m_axisType), facingTransform.position.GetDirection(Camera.main.transform.position));
				percent = 1f - UnityUtil.GetPercent(angle, m_angleLimits.x, m_angleLimits.y);
			}
		}

		#endregion
	}
}