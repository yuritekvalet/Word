using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace WordProcessor.Service
{
	public interface ITextDicionary
	{
		Task<IDictionary<string, int>> GetDictionaryFromFile(string pathFile);
		IEnumerable<DictionaryWord> GetDictionaryWord(IDictionary<string, int> dictionary);
	}
}
