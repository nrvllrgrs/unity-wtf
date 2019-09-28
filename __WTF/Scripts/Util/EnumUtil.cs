using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumUtil
{
	public static T Random<T>(Random random = null)
	{
		return GetValues<T>()
			.Random(random);
	}

	public static IEnumerable<T> GetValues<T>()
	{
		return Enum.GetValues(typeof(T)).Cast<T>();
	}

	public static void Iterate<T>(Action<T> action)
	{
		foreach (var item in GetValues<T>())
		{
			action(item);
		}
	}
}