using Ludiq;

[IncludeInSettings(true)]
public static class NumberExt
{
	public static bool Between(this float value, float min, float max)
	{
		return min <= value && value <= max;
	}

	public static bool Between(this int value, int min, int max)
	{
		return min <= value && value <= max;
	}

	public static float Wrap(this float value, float maxDouble)
	{
		value = Mod(value, maxDouble);
		if (value > maxDouble * 0.5f)
		{
			value -= maxDouble;
		}
		return value;
	}

	public static float WrapAngle(this float value)
	{
		return value.Wrap(360f);
	}

	public static float Mod(this float a, float b)
	{
		return (a % b + b) % b;
	}

	public static int Mod(this int a, int b)
	{
		return (a % b + b) % b;
	}
}
