using FLM.DAL.Contracts;
using FLM.DAL.EFCore.Extensions;
using FLM.DAL.Extensions;
using FLM.Model.Base;
using FLM.Model.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FLM.DAL.EFCore.Repositories.Base
{
	public class BaseEFRepository<T> : IRepository<T> where T : class, IEntity
	{
		protected readonly FootballDbContext Context;
		protected readonly IUserResolver UserResolver;
		protected DbSet<T> DbSet;
		private IUserInfo _userInfo;

		public BaseEFRepository(FootballDbContext context, IUserResolver userResolver)
		{
			Context = context;
			UserResolver = userResolver;
			DbSet = Context.Set<T>();
		}

		protected IUserInfo UserInfo
		{
			get { return _userInfo ?? (_userInfo = UserResolver?.GetUser()); }
		}

		// Get All Items Without Paging
		public IQueryable<T> GetItems()
		{
			return DbSet;
		}

		// Get Items With Paging
		public IQueryable<T> GetItems(int pageSize = 10, int pageNumber = 1)
		{
			return DbSet.Paging(pageSize, pageNumber);
		}

		public virtual async Task<T> GetItemByIdAsync(int id, params Expression<Func<T, object>>[] includes)
		{
			return await DbSet.IncludeMultiple(includes).FirstOrDefaultAsync(item => item.Id == id);
		}

		public virtual async Task<int> AddItemAsync(T item)
		{
			AddItem(item);
			return await CommitChangesAsync();
		}

		public virtual async Task<int> UpdateItemAsync(T item)
		{
			UpdateItem(item);
			return await CommitChangesAsync();
		}

		public virtual async Task<int> RemoveItemAsync(T item)
		{
			DbSet.Remove(item);
			return await CommitChangesAsync();
		}

		protected virtual void AddItem(T item)
		{
			if (item is IAuditEntity auditEntity)
			{
				auditEntity.CreationUser = UserInfo?.Email;

				if (!auditEntity.CreationDateTime.HasValue)
				{
					auditEntity.CreationDateTime = DateTime.Now;
				}
			}

			DbSet.Add(item);
		}

		protected virtual void UpdateItem(T item)
		{
			if (item is IAuditEntity auditEntity)
			{
				auditEntity.LastUpdateUser = UserInfo?.Email;
				auditEntity.LastUpdateDateTime = DateTime.Now;
			}

			DbSet.Update(item);
		}

		public void CommitChanges()
		{
			Context.SaveChanges();
		}

		public Task<int> CommitChangesAsync()
		{
			return Context.SaveChangesAsync();
		}

		public async Task<int> AddItemsAsync(IEnumerable<T> items)
		{
			//await DbSet.AddRangeAsync(items);
			foreach (var item in items)
			{
				AddItem(item);
			}
			return await CommitChangesAsync();
		}

		public async Task<int> RemoveItemsAsync(IEnumerable<T> items)
		{
			DbSet.RemoveRange(items);
			return await CommitChangesAsync();
		}
	}
}