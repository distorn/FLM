using System.Text.RegularExpressions;

namespace FLM.BL.Extensions
{
	public static class StringExtensions
	{
		public static string RemoveExtraSpaces(this string source)
		{
			return Regex.Replace(source, @"\s{2,}", " ").Trim();
		}

		public static string[] GetWords(this string source)
		{
			return source.RemoveExtraSpaces().Split(' ');
		}
	}
}