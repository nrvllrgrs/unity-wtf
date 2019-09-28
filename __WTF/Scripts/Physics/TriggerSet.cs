using UnityEngine;

namespace UnityEngine.Workshop
{
	public class TriggerSet : Set
	{
		#region Methods

		private void OnTriggerEnter(Collider other)
		{
			Add(other.gameObject);
		}

		private void OnTriggerExit(Collider other)
		{
			Remove(other.gameObject);
		}

		#endregion
	}
}
