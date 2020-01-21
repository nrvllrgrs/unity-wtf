using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class LookAt : MonoBehaviour
	{
		#region Variables

		[ShowIf("exposeTarget")]
		public Transform target;

		[ShowIf("exposeVariables")]
		public Vector3 positionOffset;

		[ShowIf("exposeVariables")]
		public Vector3 worldUp = Vector3.up;

		[ShowIf("exposeVariables")]
		public bool headingOnly = false;

		public bool reverse = false;

		#endregion

		#region Properties

		protected virtual bool exposeTarget => true;
		protected virtual bool exposeVariables => true;

		#endregion

		#region Methods

		private void Update()
		{
			if (target != null)
			{
				UpdateFacing();
			}
		}

		protected virtual void UpdateFacing()
		{
			var position = target.position + positionOffset;
			if (headingOnly)
			{
				position.y = transform.position.y;
			}

			if (!reverse)
			{
				transform.LookAt(position, worldUp.normalized);
			}
			else
			{
				transform.LookAt(2f * transform.position - position, worldUp.normalized);
			}
		}

		#endregion
	}
}
