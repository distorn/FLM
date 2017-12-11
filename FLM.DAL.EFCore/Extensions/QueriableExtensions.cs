using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FLM.DAL.EFCore.Extensions
{
	public static class QueriableExtensions
	{
		public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
		{
			if (includes != null)
			{
				query = includes.Aggregate(query, (current, include) => current.Include(include));
			}
			return query;
		}
	}
}