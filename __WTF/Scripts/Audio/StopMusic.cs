using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public class StopMusic : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private bool m_fadeOut;

		[SerializeField, MinValue(0f), ShowIf("m_fadeOut")]
		private float m_duration = 1f;

		[SerializeField, ShowIf("m_fadeOut")]
		private AnimationCurve m_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		private bool m_stopOnAwake;

		#endregion

		#region Methods

		private void Start()
		{
			if (m_stopOnAwake)
			{
				Stop();
			}
		}

		public void Stop()
		{
			if (!MusicManager.Exists)
			{
				Debug.LogError("MusicManager is undefined!");
				return;
			}

			if (!m_fadeOut)
			{
				MusicManager.Instance.Stop();
			}
			else
			{
				MusicManager.Instance.FadeOut(m_duration, m_curve);
			}
		}

		#endregion
	}
}
