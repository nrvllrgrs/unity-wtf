using UnityEngine.EventSystems;

namespace UnityEngine.Workshop
{
	public static class MouseUtil
	{
		#region Variables

		private static MouseEventListener m_listener;
		private static System.EventHandler m_clicked, m_doubleClicked;

		#endregion

		#region Events

		public static event System.EventHandler Clicked
		{
			add
			{
				GetListener();
				m_clicked += value;
			}
			remove { m_clicked -= value; }
		}

		public static event System.EventHandler DoubleClicked
		{
			add
			{
				GetListener();
				m_doubleClicked += value;
			}
			remove { m_doubleClicked -= value; }
		}

		#endregion

		#region Methods

		private static MouseEventListener GetListener()
		{
			if (m_listener == null)
			{
				var obj = new GameObject("MouseEventListener");
				obj.hideFlags |= HideFlags.HideAndDontSave | HideFlags.HideInInspector;

				m_listener = obj.AddComponent<MouseEventListener>();
			}
			return m_listener;
		}

		#endregion

		#region Structures

		public class MouseEventListener : MonoBehaviour
		{

			#region Variables

			private float m_lastClickTimestamp = Mathf.NegativeInfinity;

			private const float DOUBLE_CLICK_THRESHOLD = 0.2f;

			#endregion

			#region Methods

			private void Update()
			{
				// Check clicking mouse AND not on UI element
				if (Input.GetKeyDown(KeyCode.Mouse0) && EventSystem.current?.currentSelectedGameObject == null)
				{
					if (Time.time - DOUBLE_CLICK_THRESHOLD > m_lastClickTimestamp)
					{
						m_clicked?.Invoke(this, System.EventArgs.Empty);
					}
					else
					{
						m_doubleClicked?.Invoke(this, System.EventArgs.Empty);
					}

					m_lastClickTimestamp = Time.time;
				}
			}

			#endregion
		}

		#endregion
	}
}