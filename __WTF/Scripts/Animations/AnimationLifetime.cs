namespace UnityEngine.Workshop
{
	[RequireComponent(typeof(Animation))]
	public class AnimationLifetime : MonoBehaviour
	{
		#region Variables

		private Animation m_animation;
		private bool m_prevIsPlaying;

		#endregion

		#region Properties

		public new Animation animation => this.GetComponent(ref m_animation);

		#endregion

		#region Methods

		private void Update()
		{
			if (!animation.isPlaying && m_prevIsPlaying)
			{
				GameObjectUtil.Destroy(gameObject);
			}

			m_prevIsPlaying = animation.isPlaying;
		}

		#endregion
	}
}