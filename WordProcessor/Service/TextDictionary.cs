using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.Service;

namespace WordProcessor.Service
{
	public class TextDictionary : ITextDicionary
	{
		private readonly IReadTextFile _file;
		public TextDictionary()
		{
			_file = new ReadTextFile();
		}
		public async Task<IDictionary<string, int>> GetDictionaryFromFile(string pathFile)
		{
			return await _file.ParseTextAsync(pathFile, ReadTextFile.ParamSortDic.ValueDesc);
		}

		public IEnumerable<DictionaryWord> GetDictionaryWord(IDictionary<string, int> dictionary)
		{
			return dictionary.Select(pair => new DictionaryWord
			{
				Count = pair.Value,
				Word = pair.Key
			});
		}
	}
}
