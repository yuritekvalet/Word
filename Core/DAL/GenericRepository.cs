using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Core.DAL
{
	public sealed class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
	{
		private readonly WordContext _context;
		private readonly DbSet<TEntity> _dbSet;
		public GenericRepository(WordContext context)
		{
			_context = context;
			_dbSet = context.Set<TEntity>();
			ReplacementDataDirectory();
		}
		private void ReplacementDataDirectory()
		{
			string relative = @"..\..\App_Data";
			string absolute = Path.GetFullPath(relative);
			AppDomain.CurrentDomain.SetData("DataDirectory", absolute);

			Database.SetInitializer(new
				MigrateDatabaseToLatestVersion<WordContext, Migrations.Configuration>()
			);
		}
		public IEnumerable<TEntity> Get(
			Expression<Func<TEntity, bool>> filter = null,
			int take = 0,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
		{
			IQueryable<TEntity> query = _dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			var queryOrder = orderBy?.Invoke(query) ?? query;
			return take > 0 ? queryOrder.Take(take).ToList() : queryOrder.ToList();
		}
		public void AddRange(IEnumerable<TEntity> entities)
		{
			_dbSet.AddRange(entities);
		}
		public void Clear()
		{
			TEntity[] entities = _dbSet.ToArray();
			RemoveRange(entities);
		}
		private void RemoveRange(TEntity[] entities)
		{
			foreach (TEntity entity in entities)
			{
				if (_context.Entry(entity).State == EntityState.Detached)
				{
					_dbSet.Attach(entity);
				}
				_dbSet.Remove(entity);
			}
		}
		public void Update(IEnumerable<TEntity> entitys)
		{
			foreach (var entity in entitys)
			{
				_dbSet.Attach(entity);
				_context.Entry(entity).State = EntityState.Modified;
			}

		}
	}
}
