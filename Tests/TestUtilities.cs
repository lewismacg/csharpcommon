using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CSharpCommon.Tests
{
	public static class TestUtilities
	{
		internal static CSharpCommonTestDbContext GetInMemoryContext()
		{
			var optionsBuilder = new DbContextOptionsBuilder<CSharpCommonTestDbContext>();

			optionsBuilder.UseSqlite(CreateInMemoryDatabase());

			var context = new CSharpCommonTestDbContext(optionsBuilder.Options);

			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			return context;
		}

		internal static DbConnection CreateInMemoryDatabase()
		{
			var connection = new SqliteConnection("Filename=:memory:");

			connection.Open();

			return connection;
		}
	}
}
