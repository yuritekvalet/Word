using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Service
{
	public interface IReadTextFile
	{
		Task<IDictionary<string, int>> ParseTextAsync(string filePath, ReadTextFile.ParamSortDic dic);
	}

}
