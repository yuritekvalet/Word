using System;
using System.Collections.Generic;
using System.Linq;

namespace WordProcessor.Service
{
	public class ServiceConfiguration
	{
		private readonly IEnumerable<string> _args;
		public ServiceConfiguration(IEnumerable<string> args)
		{
			_args = args;
		}

		public string GetFirstStringKey()
		{
			return _args
				.Select(arg =>arg.Substring(arg.IndexOf("/", StringComparison.Ordinal) + 1, arg.IndexOf(":", StringComparison.Ordinal) - 1))
				.FirstOrDefault();
		}
		public string GetStringParametr(string parametrName)
		{
			parametrName = $"/{parametrName}:";
			return _args
				.Where(arg => arg.StartsWith(parametrName))
				.Select(arg => arg.Substring(parametrName.Length))
				.FirstOrDefault();
		}
	}
}
