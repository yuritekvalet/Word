using System.Collections.Generic;
using Core.DAL;
using Core.Models;

namespace WordProcessor.Service
{
	public class DictonaryWordRepository : IRepository<DictionaryWord>
	{
		public void AddRange(IEnumerable<DictionaryWord> entities)
		{
			using (var unitOfWork = new UnitOfWork<DictionaryWord>())
			{
				unitOfWork.DictionaryWordRepository.AddRange(entities);
				unitOfWork.Save();
			}
		}
		public void Clear()
		{
			using (var unitOfWork = new UnitOfWork<DictionaryWord>())
			{
				unitOfWork.DictionaryWordRepository.Clear();
				unitOfWork.Save();
			}
		}
		public void Update(IEnumerable<DictionaryWord> entity)
		{
			using (var unitOfWork = new UnitOfWork<DictionaryWord>())
			{
				unitOfWork.DictionaryWordRepository.Update(entity);
				unitOfWork.Save();
			}
		}
	}
}
