public static class StringExt
{
	public static string ToCamel(this string text)
	{
		return (char.ToLowerInvariant(text[0]) + text.Substring(1)).Replace(" ", string.Empty);
	}
}