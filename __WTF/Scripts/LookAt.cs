using UnityEngine;

namespace UnityEngine.Workshop
{
	public class LookAt : MonoBehaviour
	{
		#region Variables

		public Transform target;
		public Vector3 positionOffset;
		public Vector3 worldUp = Vector3.up;

		#endregion

		#region Methods

		private void Update()
		{
			if (target != null)
			{
				transform.LookAt(target.position + positionOffset, worldUp.normalized);
			}
		}

		#endregion
	}
}
