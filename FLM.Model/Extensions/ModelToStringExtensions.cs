using FLM.Model.Entities;

namespace FLM.Model.Extensions
{
	public static class ModelToStringExtensions
	{
		public static string GetDisplayName(this Player item)
		{
			return item != null ? $"{item.LastName} {item.FirstName}" : null;
		}

		public static string GetDisplayName(this Team item)
		{
			return item != null ? $"{item.City} {item.Name}" : null;
		}

		public static string GetDisplayName(this League item)
		{
			return item != null ? $"{item.Name} {item.Season}" : null;
		}
	}
}