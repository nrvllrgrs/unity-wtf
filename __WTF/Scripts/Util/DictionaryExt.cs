using System.Collections.Generic;

public static class DictionaryExt
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="K"></typeparam>
	/// <param name="dict"></param>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns>Indicates whether key previously existed</returns>
	public static bool Set<T, K>(this Dictionary<T, K> dict, T key, K value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key] = value;
			return true;
		}

		dict.Add(key, value);
		return false;
	}

	public static K Get<T, K>(this Dictionary<T, K> dict, T key)
	{
		return dict.Get(key, default);
	}

	public static K Get<T, K>(this Dictionary<T, K> dict, T key, K fallback)
	{
		if (dict.ContainsKey(key))
		{
			return dict[key];
		}

		dict.Add(key, fallback);
		return fallback;
	}

	public static Dictionary<T, K> Initialize<T, K>(Dictionary<T, K> dict)
		where T : struct, System.IConvertible
	{
		if (dict == null)
		{
			dict = new Dictionary<T, K>();
		}

		var temp = new Dictionary<T, K>();
		EnumUtil.Iterate<T>((key) =>
		{
			if (dict.ContainsKey(key))
			{
				temp.Add(key, dict[key]);
			}
			else
			{
				temp.Add(key, default);
			}
		});

		return temp;
	}
}
