using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public sealed class Identifier : MonoBehaviour
	{
		#region Variables

		private int? m_instanceId;

		#endregion

		#region Properties

		[ShowInInspector, ReadOnly]
		public int id
		{
			get
			{
				if (!m_instanceId.HasValue)
				{
					m_instanceId = gameObject.GetInstanceID();
				}

				return m_instanceId.Value;
			}
		}

		#endregion

		#region Methods

		private void Awake()
		{
			this.WaitUntil(
				() => IdentifierContext.Exists,
				() => IdentifierContext.Register(this));
		}

		private void OnDestroy()
		{
			if (IdentifierContext.Exists)
			{
				IdentifierContext.Unregister(this);
			}
		}

		#endregion
	}
}