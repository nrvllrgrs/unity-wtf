using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ludiq;

[IncludeInSettings(true)]
public static class Vector3Ext
{
	public static float SqrDistance(this Vector3 a, Vector3 b)
	{
		return a.GetDirection(b).sqrMagnitude;
	}

	public static float HorizontalDistance(this Vector3 a, Vector3 b)
	{
		return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
	}

	public static float VerticalDistance(this Vector3 a, Vector3 b)
	{
		return Mathf.Abs(b.y - a.y);
	}

	public static Vector3 MidPoint(this Vector3 a, Vector3 b, Space space = Space.World)
	{
		Vector3 results = new Vector3(
			(b.x - a.x) / 2f,
			(b.y - a.y) / 2f,
			(b.z - a.z) / 2f);

		if (space == Space.World)
		{
			results += a;
		}

		return results;
	}

	public static Ray GetRay(this Vector3 src, Vector3 dst)
	{
		return new Ray(src, src.GetDirection(dst));
	}

	public static Vector3 GetDirection(this Vector3 src, Vector3 dst)
	{
		return dst - src;
	}

	public static Vector3 GetHorizontalDirection(this Vector3 src, Vector3 dst)
	{
		Vector3 dir = src.GetDirection(dst);
		dir.y = 0f;

		return dir;
	}

	public static Vector3 GetPointOnLineByDistance(this Vector3 a, Vector3 b, float distance)
	{
		return a + (b - a).normalized * distance;
	}

	public static float NormalizedDistance(this Vector3 src, Vector3 dst, Vector3 point)
	{
		Vector3 projection = Vector3.Project(src.GetDirection(point), src.GetDirection(dst));
		return Mathf.Clamp01(projection.magnitude / Vector3.Distance(src, dst));
	}

	public static Vector3 GetTangent(this Vector3 direction)
	{
		Vector3 tangent = Vector3.Cross(direction, Vector3.forward);
		if (tangent.sqrMagnitude == 0f)
		{
			tangent = Vector3.Cross(direction, Vector3.up);
		}

		return tangent;
	}

	public static Vector2 ConvertToVector2(this Vector3 a)
	{
		return new Vector2(a.x, a.y);
	}

	public static Vector3 ConvertToVector3(this Vector4 a)
	{
		return new Vector3(a.x, a.y, a.z);
	}

	public static Vector4 ConvertToVector4(this Vector3 a)
	{
		return new Vector4(a.x, a.y, a.z, 0f);
	}

	public static Vector3 Abs(this Vector3 a)
	{
		return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
	}

	public static Vector3 WrapAngle(this Vector3 a)
	{
		return new Vector3(a.x.WrapAngle(), a.y.WrapAngle(), a.z.WrapAngle());
	}

	public static Vector3 ModAngle(this Vector3 a)
	{
		return new Vector3(a.x.Mod(360f), a.y.Mod(360f), a.z.Mod(360f));
	}
	
	public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
	{
		return new Vector3(
			Mathf.Clamp(value.x, min.x, max.x),
			Mathf.Clamp(value.y, min.y, max.y),
			Mathf.Clamp(value.z, min.z, max.z));
	}

	public static bool Approximately(this Vector2 a, Vector2 b)
	{
		return Mathf.Approximately(a.x, b.x)
			&& Mathf.Approximately(a.y, b.y);
	}

	public static bool Approximately(this Vector3 a, Vector3 b)
	{
		return Mathf.Approximately(a.x, b.x)
			&& Mathf.Approximately(a.y, b.y)
			&& Mathf.Approximately(a.z, b.z);
	}

	public static Vector3 Center(this IEnumerable<Vector3> points)
	{
		if (!points.Any())
		{
			return Vector3.zero;
		}
		else if (points.Count() == 1)
		{
			return points.ElementAt(0);
		}
		else
		{
			var bounds = new Bounds(points.ElementAt(0), Vector3.zero);
			for (int i = 1; i < points.Count(); ++i)
			{
				bounds.Encapsulate(points.ElementAt(i));
			}

			return bounds.center;
		}
	}

	public static void Split(this Vector3 value, out float x, out float y, out float z)
	{
		x = value.x;
		y = value.y;
		z = value.z;
	}

	public static Vector3 Sum(this IEnumerable<Vector3> list)
	{
		Vector3 total = Vector3.zero;
		foreach (var item in list)
		{
			total += item;
		}

		return total;
	}
}
