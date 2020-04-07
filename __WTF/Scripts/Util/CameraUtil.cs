using UnityEngine;

public static class CameraUtil
{
	#region Variables

	private static Camera m_main;

	#endregion

	#region Properties

	public static Camera main
	{
		get
		{
			if (m_main == null)
			{
				m_main = Camera.main;
			}
			return m_main;
		}
	}

	#endregion
}
