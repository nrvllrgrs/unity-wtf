using UnityEngine;

namespace UnityEngine
{
	public enum AxisType
	{
		Right,
		Left,
		Up,
		Down,
		Forward,
		Back,
	}
}

public static class AxisExt
{
	public static Vector3 GetAxis(this Transform transform, AxisType axisType)
	{
		switch (axisType)
		{
			case AxisType.Forward:
				return transform.forward;

			case AxisType.Up:
				return transform.up;

			case AxisType.Right:
				return transform.right;

			case AxisType.Back:
				return -transform.forward;

			case AxisType.Down:
				return -transform.up;

			case AxisType.Left:
				return -transform.right;
		}

		return Vector3.zero;
	}
}
