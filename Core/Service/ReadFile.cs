using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Service
{
	public class ReadTextFile : IReadTextFile
	{
		public enum ParamSortDic
		{
			KeyAsc,
			KeyDesc,
			ValueAsc,
			ValueDesc
		}

		readonly string[] _pretext =
		{
			"в", "из", "к", "у", "по", "из-за", "по-над",
			"под", "около", "вокруг", "перед", "возле", "и", "над", "все", "она",
			"вот", "это", "где", "этот", "так", "его"
		};

		readonly Dictionary<ParamSortDic, Func<IDictionary<string, int>, IOrderedEnumerable<KeyValuePair<string, int>>>> _sorting =
			new Dictionary<ParamSortDic, Func<IDictionary<string, int>, IOrderedEnumerable<KeyValuePair<string, int>>>>
			{
				{ParamSortDic.KeyAsc,word=> word.OrderBy(key=>key.Key)},
				{ParamSortDic.KeyDesc,word=>word.OrderByDescending(key=>key.Key)},
				{ParamSortDic.ValueAsc,word=>word.OrderBy(key=>key.Value)},
				{ParamSortDic.ValueDesc,word=>word.OrderByDescending(key=>key.Value)},
			};

		private static readonly char[] Separators = { ' ' };
		public async Task<IDictionary<string, int>> ParseTextAsync(string filePath, ParamSortDic dic)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				throw new ArgumentNullException(nameof(filePath));
			}

			if (Path.GetExtension(filePath) != ".txt")
			{
				throw new ArgumentException("Укажите текстовой файл");
			}

			var wordCount = new ConcurrentDictionary<string, int>();
			const int numberWordToWrite = 3;
			const int beginingLengthWord = 2;
			const int endLengthWord = 15;

			using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
			{
				using (var streamReader = new StreamReader(fileStream))
				{
					string line;
					while ((line = await streamReader.ReadLineAsync()) != null)
					{
						var lineModifyLower = Regex
							.Replace(line, "[^а-яА-я \\dictionary]", "")
							.ToLower();
						var words = lineModifyLower
							.Split(Separators, StringSplitOptions.RemoveEmptyEntries)
							.Where(value => !_pretext.Contains(value))
							.Where(word => word.Length > beginingLengthWord)
							.Where(word => word.Length < endLengthWord);

						Parallel.ForEach(words, word =>
						{
							wordCount.AddOrUpdate(word, 1, (key, value) => value >= numberWordToWrite ? value : value + 1);
						});
					}
				}
			}
			return _sorting[dic](wordCount).ToDictionary(keyValuePair => keyValuePair.Key, valuePair => valuePair.Value);
		}
	}
}
