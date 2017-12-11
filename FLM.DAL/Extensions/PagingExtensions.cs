using System;
using System.Linq;

namespace FLM.DAL.Extensions
{
	public static class PagingExtensions
	{
		public static IQueryable<T> Paging<T>(this IQueryable<T> query, Int32 pageSize = 0, Int32 pageNumber = 0) where T : class
		{
			return pageSize > 0 && pageNumber > 0 ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize) : query;
		}

	}
}