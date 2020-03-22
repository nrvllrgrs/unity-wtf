namespace UnityEngine.Workshop
{
	[RequireComponent(typeof(Animation))]
	public class AnimationPoolItem : MonoBehaviour, IPoolItem
	{
		#region Variables

		private Animation m_animation;

		#endregion

		#region Properties

		public new Animation animation => this.GetComponent(ref m_animation);

		#endregion

		#region Methods

		public void Restart()
		{
			animation.enabled = true;
		}

		#endregion
	}
}
