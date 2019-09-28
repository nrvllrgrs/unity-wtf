using UnityEngine;

namespace UnityEngine.Workshop
{
	public class Trigger2DSet : Set
	{
		#region Methods

		private void OnTriggerEnter2D(Collider2D collision)
		{
			Add(collision.gameObject);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			Remove(collision.gameObject);
		}

		#endregion
	}
}
