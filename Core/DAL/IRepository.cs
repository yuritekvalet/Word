using System.Collections.Generic;

namespace Core.DAL
{
	public interface IRepository<in TEntity> where TEntity : class, new()
	{
		void AddRange(IEnumerable<TEntity> entities);
		void Clear();
		void Update(IEnumerable<TEntity> entity);
	}
}
