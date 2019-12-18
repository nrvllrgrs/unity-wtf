using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class FaceCamera_UnityEvents : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private FaceCameraEventListener m_faceCamera;

		#endregion

		#region Events

		[FoldoutGroup("Events")]
		public UnityEvent onFacingStarted = new UnityEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onFacingChanged = new UnityEvent();

		[FoldoutGroup("Events")]
		public UnityEvent onFacingStopped = new UnityEvent();

		#endregion

		#region Properties

		public FaceCameraEventListener faceCamera => this.GetComponent(ref m_faceCamera);

		#endregion

		#region Methods

		private void OnEnable()
		{
			faceCamera.FacingStarted += FacingStarted;
			faceCamera.FacingChanged += FacingChanged;
			faceCamera.FacingStopped += FacingStopped;
		}

		private void OnDisable()
		{
			faceCamera.FacingStarted -= FacingStarted;
			faceCamera.FacingChanged -= FacingChanged;
			faceCamera.FacingStopped -= FacingStopped;
		}

		private void FacingStarted(object sender, System.EventArgs e)
		{
			onFacingStarted.Invoke();
		}

		private void FacingChanged(object sender, System.EventArgs e)
		{
			onFacingChanged.Invoke();
		}

		private void FacingStopped(object sender, System.EventArgs e)
		{
			onFacingStopped.Invoke();
		}

		#endregion
	}
}
