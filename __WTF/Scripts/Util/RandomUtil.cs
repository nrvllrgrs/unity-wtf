using System.Collections.Generic;
using System.Linq;
using Ludiq;

[IncludeInSettings(true)]
public static class RandomUtil
{
	public static int Next(this System.Random random, int min, int max, IEnumerable<int> excludedIndices)
	{
		excludedIndices = excludedIndices.OrderBy(x => x);

		int value = min + random.Next(max - min - excludedIndices.Count());
		for (int i = 0; i < excludedIndices.Count(); ++i)
		{
			if (excludedIndices.ElementAt(i) > value)
			{
				return value;
			}
			++value;
		}

		return value;
	}
	
	public static float Next(this System.Random random, float min, float max)
	{
		return (float)(random.NextDouble() * (max - min) + min);
	}

	public static float NextVariance(this System.Random random, float average, float variance)
	{
		float delta = average * variance;
		return random.Next(average - delta, average + delta);
	}

	public static T Random<T>(this IList<T> availableValues, System.Random random = null, bool removeValue = false)
	{
		// Randomly select index from available indices
		int index = random != null
			? random.Next(0, availableValues.Count)
			: UnityEngine.Random.Range(0, availableValues.Count);

		// Remember value from available indicies
		T value = availableValues[index];

		if (removeValue)
		{
			// Remove selected index from available indices so not used again
			availableValues.RemoveAt(index);
		}

		return value;
	}

	public static T Random<T>(this IEnumerable<T> availableValues, System.Random random = null)
	{
		return availableValues.ToList().Random(random, false);
	}

	public static int WeightedRandomIndex(this IEnumerable<float> items, System.Random random = null)
	{
		float totalWeights = items.Sum();
		float value = random != null
			? (float)random.NextDouble() * totalWeights
			: UnityEngine.Random.Range(0f, totalWeights);

		int count = items.Count();
		for (int i = 0; i < count - 1; ++i)
		{
			var weight = items.ElementAt(i);
			if (value < weight)
			{
				return i;
			}

			value -= weight;
		}

		return items.Count() - 1;
	}

	public static T WeightedRandom<T>(this IEnumerable<IWeightedItem<T>> items, System.Random random = null)
	{
		return items.ElementAt(items.Select(x => x.weight)
			.WeightedRandomIndex(random)).item;
	}

	public static IList<T> Shuffle<T>(this IList<T> list, System.Random random = null)
	{
		List<T> result = new List<T>(list);

		int index = result.Count;
		while (index > 1)
		{
			--index;
			int randomIndex = random != null
				? random.Next(0, index + 1)
				: UnityEngine.Random.Range(0, index + 1);

			T value = result[randomIndex];
			result[randomIndex] = result[index];
			result[index] = value;
		}

		return result;
	}

	/// <summary>
	/// Random value in normal distribution
	/// </summary>
	/// <returns>Returns value between -1 to 1 (excluding 0)</returns>
	public static float Gaussian()
	{
		float v1, v2, s;
		do
		{
			v1 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
			v2 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
			s = v1 * v1 + v2 * v2;
		} while (s >= 1.0f || s == 0f);

		s = UnityEngine.Mathf.Sqrt((-2.0f * UnityEngine.Mathf.Log(s)) / s);
		return v1 * s;
	}

	/// <summary>
	/// Random value in normal distribution
	/// </summary>
	/// <param name="mean"></param>
	/// <param name="stdDeviation"></param>
	/// <returns></returns>
	public static float Gaussian(float mean, float stdDeviation)
	{
		return mean + Gaussian() * stdDeviation;
	}
}

public interface IWeightedItem<T>
{
	T item { get; }
	float weight { get; }
}