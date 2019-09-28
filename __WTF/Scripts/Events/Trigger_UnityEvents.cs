using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	[System.Serializable]
	public class TriggerEvent : UnityEvent<Collider>
	{ }

	public class Trigger_UnityEvents : MonoBehaviour
	{
		#region Variables

		private Collider m_collider;

		#endregion

		#region Events

		[FoldoutGroup("Events")]
		public TriggerEvent onTriggerEnter = new TriggerEvent();

		[FoldoutGroup("Events")]
		public TriggerEvent onTriggerStay = new TriggerEvent();

		[FoldoutGroup("Events")]
		public TriggerEvent onTriggerExit = new TriggerEvent();

		#endregion

		#region Properties

		public new Collider collider
		{
			get
			{
				if (m_collider == null)
				{
					m_collider = GetComponent<Collider>();
				}
				return m_collider;
			}
		}

		#endregion

		#region Methods

		private void OnTriggerEnter(Collider other)
		{
			onTriggerEnter.Invoke(other);
		}

		private void OnTriggerStay(Collider other)
		{
			onTriggerStay.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			onTriggerExit.Invoke(other);
		}

		#endregion
	}
}