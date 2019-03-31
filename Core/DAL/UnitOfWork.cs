using System;

namespace Core.DAL
{
	public sealed class UnitOfWork<TEntity> : IDisposable where TEntity : class, new()
	{
		private readonly WordContext _context = new WordContext();
		private GenericRepository<TEntity> _wordContextRepository;
		public GenericRepository<TEntity> DictionaryWordRepository
		{
			get
			{
				return _wordContextRepository ?? new GenericRepository<TEntity>(_context);
			}
		}
		public void Save()
		{
			_context.SaveChanges();
		}
		private bool _disposed;

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			_disposed = true;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
