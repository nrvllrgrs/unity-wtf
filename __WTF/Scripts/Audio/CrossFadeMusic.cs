using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class CrossFadeMusic : MonoBehaviour
	{
		#region Variables

		[SerializeField, Required]
		private AudioClip m_music;

		[SerializeField, MinValue(0f)]
		private float m_duration = 1f;

		[SerializeField]
		private AnimationCurve m_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		private bool m_playOnAwake;

		#endregion

		#region Methods

		private void Start()
		{
			if (m_playOnAwake)
			{
				CrossFade();
			}
		}

		public void CrossFade()
		{
			this.WaitUntil(
				() => MusicManager.Exists,
				() => MusicManager.Instance.CrossFade(m_music, m_duration, m_curve));
		}

		#endregion
	}
}
