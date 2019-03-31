using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Core.Models;

namespace Core.DAL
{
	public class WordContext : DbContext
	{
		public WordContext() : base("WordContext")
		{
		}
		public DbSet<DictionaryWord> DictionaryWords { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}
