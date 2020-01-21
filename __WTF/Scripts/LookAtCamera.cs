using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class LookAtCamera : LookAt
	{
		#region Variables

		[HideIf("headingOnly"), PropertyOrder(-1)]
		public bool parallelCameraFacing;

		#endregion

		#region Properties

		protected override bool exposeTarget => false;
		protected override bool exposeVariables => !parallelCameraFacing;

		#endregion

		#region Methods

		protected virtual void OnEnable()
		{
			this.WaitUntil(
				() => { return Camera.main != null; },
				() => { target = Camera.main.transform; });
		}

		protected override void UpdateFacing()
		{
			if (headingOnly || !parallelCameraFacing)
			{
				base.UpdateFacing();
			}
			else if (!reverse)
			{
				transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
			}
			else
			{
				transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
			}
		}

		#endregion
	}
}
