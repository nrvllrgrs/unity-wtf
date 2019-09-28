using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class PlayMusic : MonoBehaviour
	{
		#region Variables

		[SerializeField, Required]
		private AudioClip m_music;

		[SerializeField]
		private bool m_fadeIn;

		[SerializeField, MinValue(0f), ShowIf("m_fadeIn")]
		private float m_duration = 1f;

		[SerializeField, ShowIf("m_fadeIn")]
		private AnimationCurve m_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		private bool m_playOnAwake;

		#endregion

		#region Methods

		private void Start()
		{
			if (m_playOnAwake)
			{
				Play();
			}
		}

		public void Play()
		{
			if (!MusicManager.Exists)
			{
				Debug.LogError("MusicManager is undefined!");
				return;
			}

			if (!m_fadeIn)
			{
				MusicManager.Instance.Play(m_music);
			}
			else
			{
				MusicManager.Instance.FadeIn(m_music, m_duration, m_curve);
			}
		}

		#endregion
	}
}