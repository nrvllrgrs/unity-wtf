namespace UnityEngine.Workshop
{
	public class LookAtCamera : MonoBehaviour
	{
		#region Variables

		public Vector3 positionOffset;
		public Vector3 worldUp = Vector3.up;

		private LookAt m_lookAt;

		#endregion

		#region Methods

		protected virtual void OnEnable()
		{
			if (gameObject.TryAddComponent(out m_lookAt))
			{
				m_lookAt.hideFlags |= HideFlags.HideInHierarchy | HideFlags.HideInInspector;
			}
			m_lookAt.enabled = true;

			this.WaitUntil(
				() => { return Camera.main != null; },
				() =>
				{
					m_lookAt.target = Camera.main.transform;
					m_lookAt.positionOffset = positionOffset;
					m_lookAt.worldUp = worldUp;
				});
		}

		protected virtual void OnDisable()
		{
			if (m_lookAt != null)
			{
				m_lookAt.enabled = false;
			}
		}

		#endregion
	}
}
