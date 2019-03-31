using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.DAL;
using Core.Models;
using WordProcessor.Service;

namespace WordProcessor
{
	static class Program
	{
		enum CommandParametrs
		{
			ClearDictionary,
			CreateDictonary,
			UpdateDictionary
		}
		/// <summary>
		/// Для добавлени, обновления записей в бд программу необходимо запускать 
		/// с праметром шаблона /MyOperation:pathToFile
		/// Sample:/CreateDictonary:C:\Тест\TestFile.txt.
		/// Для удаления с параметром /ClearDictionary:pathToFile
		/// </summary>
		static void Main(string[] args)
		{
			if (args.Length > 1)
			{
				Console.WriteLine(@"Запустить приложение можно только с одним параметром");
				Console.ReadKey();
				Environment.Exit(0);
			}

			if (args.Length == 1)
			{
				var dictonaryWordRepository = new DictonaryWordRepository();
				var argCommand = args[0];
				if (argCommand.Contains(CommandParametrs.ClearDictionary.ToString()))
				{
					dictonaryWordRepository.Clear();
					Console.WriteLine(@"Таблица DictionaryWord была очищена");
					Thread.Sleep(5000);
				}

				if (argCommand.Contains(CommandParametrs.CreateDictonary.ToString()) ||
					argCommand.Contains(CommandParametrs.UpdateDictionary.ToString()))
				{
					var serviceConfig = new ServiceConfiguration(args);

					var stringCommandParametr = serviceConfig.GetFirstStringKey();
					var pathFile = serviceConfig.GetStringParametr(stringCommandParametr);

					var textDictionary = new TextDictionary();

					var dictionaryFromFile = textDictionary.GetDictionaryFromFile(pathFile);
					Task.WaitAll(dictionaryFromFile);
					var words = textDictionary.GetDictionaryWord(dictionaryFromFile.Result);

					var dictionaryWords = words as DictionaryWord[] ?? words.ToArray();

					if (stringCommandParametr == CommandParametrs.CreateDictonary.ToString())
					{
						dictonaryWordRepository.AddRange(dictionaryWords);
						Console.WriteLine(@"В таблицу DictionaryWord были добавлены записи");
						Thread.Sleep(5000);
					}

					if (stringCommandParametr == CommandParametrs.UpdateDictionary.ToString())
					{
						dictonaryWordRepository.Update(dictionaryWords);
						Console.WriteLine(@"В таблице DictionaryWord были обновлены записи");
						Thread.Sleep(5000);
					}
				}
			}
			if (args.Length == 0)
			{
				ConsoleKeyInfo cki;
				var stringBuilder = new StringBuilder();
				do
				{
					cki = Console.ReadKey();
					if (cki.Key != ConsoleKey.Enter)
						stringBuilder.Append(cki.KeyChar);
					else
					{
						Console.WriteLine();
						using (var unitOfWork = new UnitOfWork<DictionaryWord>())
						{
							var searchString = stringBuilder.ToString();
							var result = unitOfWork.DictionaryWordRepository.Get(word => word.Word.StartsWith(searchString)
								, 5
								, order => order.OrderByDescending(word => word.Count).ThenBy(word => word.Word));
							foreach (var word in result)
							{
								Console.WriteLine(word.Word);
							}
							stringBuilder.Clear();
						}
					}
				} while (cki.Key != ConsoleKey.Escape && cki.Key != ConsoleKey.Spacebar);
			}
		}
	}
}
