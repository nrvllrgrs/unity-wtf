using UnityEngine;
using System.Collections;

namespace UnityEngine.Workshop
{
	public abstract class Saveable : MonoBehaviour
	{
		#region Variables

		private Identifier m_identifier;

		#endregion

		#region Properties

		public Identifier identifier => this.GetComponent(ref m_identifier);
		public string guid => string.Format("{0}.{1}", identifier.id, GetType().ToString());

		#endregion

		#region Methods

		public abstract void Save(string fileName);
		public abstract void Load(string fileName);

		#endregion
	}
}
