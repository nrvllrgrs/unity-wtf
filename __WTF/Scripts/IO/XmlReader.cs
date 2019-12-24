using System.Xml.Linq;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public abstract class XmlReader : MonoBehaviour
	{
		#region Variables

		[SerializeField, Required]
		private TextAsset m_file;

		private XElement m_root;

		#endregion

		#region Properties

		public XElement root
		{
			get
			{
				if (m_root == null)
				{
					// Assign root element from document
					XDocument xml = XDocument.Parse(m_file.text);
					m_root = xml.Root;
				}
				return m_root;
			}
		}

		#endregion
	}
}
