using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ludiq;

[IncludeInSettings(true)]
public static class ListExt
{
	public static float Product(this IEnumerable<float> list)
	{
		float result = 1f;
		foreach (var value in list)
		{
			result *= value;
		}
		return result;
	}

	public static void Modify(this List<int> list, int index, int modifier, int modulo)
	{
		if (index < 0 || index >= list.Count)
		{
			throw new System.ArgumentOutOfRangeException("index");
		}

		list[index] = (list[index] + modifier + modulo) % modulo;
	}

	public static void DestroyItems(this IEnumerable<GameObject> list)
	{
		for (int i = list.Count() - 1; i >= 0; --i)
		{
			GameObjectUtil.Destroy(list.ElementAt(i));
		}
	}

	public static void DestroyItems<T>(this IEnumerable<T> list)
		where T : Component
	{
		for (int i = list.Count() - 1; i >= 0; --i)
		{
			Object.Destroy(list.ElementAt(i));
		}
	}

	public static void DestroyItemsImmediate<T>(this IEnumerable<T> list)
		where T : Object
	{
		for (int i = list.Count() - 1; i >= 0; --i)
		{
			Object.DestroyImmediate(list.ElementAt(i));
		}
	}
}
