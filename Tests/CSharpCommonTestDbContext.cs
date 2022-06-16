using Microsoft.EntityFrameworkCore;
using CSharpCommon.Tests.Data.EntityFramework.TestObjects;

namespace CSharpCommon.Tests
{
	public class CSharpCommonTestDbContext : DbContext
	{
		public DbSet<TestDbObject> TestEntities { get; set; }
		public DbSet<RelatedTestObject> RelatedEntities { get; set; }

		public CSharpCommonTestDbContext(DbContextOptions<CSharpCommonTestDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RelatedTestObject>(e =>
			{
				e.HasOne(z => z.ParentObject).WithMany(x => x.RelatedObjects);
			});

			base.OnModelCreating(modelBuilder);
		}
	}
}
