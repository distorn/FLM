using FLM.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FLM.DAL.Contracts
{
	public interface IRepository<T> where T : class, IEntity
	{
		IQueryable<T> GetItems();

		IQueryable<T> GetItems(int pageSize = 10, int pageNumber = 1);

		Task<T> GetItemByIdAsync(int id, params Expression<Func<T, object>>[] includes);

		Task<int> AddItemAsync(T item);

		Task<int> AddItemsAsync(IEnumerable<T> items);

		Task<int> UpdateItemAsync(T item);

		Task<int> RemoveItemAsync(T item);

		Task<int> RemoveItemsAsync(IEnumerable<T> items);

		void CommitChanges();

		Task<int> CommitChangesAsync();
	}
}