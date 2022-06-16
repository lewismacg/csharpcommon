using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using CSharpCommon.Data.EntityFramework;
using CSharpCommon.Tests.Data.EntityFramework.TestObjects;
using CSharpCommon.Tests.Shared.Unit;
using Xunit;

namespace CSharpCommon.Tests.Data.EntityFramework
{
	public class GenericCrudRepositoryTests : UnitTestBase
	{
		private readonly CSharpCommonTestDbContext _context;
		private readonly GenericCrudRepository<TestDbObject, int, CSharpCommonTestDbContext> _instance;

		public GenericCrudRepositoryTests()
		{
			_context = TestUtilities.GetInMemoryContext();

			_instance = new GenericCrudRepository<TestDbObject, int, CSharpCommonTestDbContext>(_context);
		}

		#region Create

		[Fact]
		public void Create()
		{
			//arrange
			const int entityId = 353;
			var entity = new TestDbObject { Id = entityId };

			//act
			var actual = _instance.Create(entity);

			//assert
			actual.Should().NotBeNull();
			actual.Should().Be(entity);
			var entityFromDb = _instance.RetrieveById(entityId);
			entityFromDb.Should().NotBeNull();
		}

		[Fact]
		public void CreateMultiple()
		{
			//arrange
			var entity1 = new TestDbObject { Id = 6 };
			var entity2 = new TestDbObject { Id = 7777 };

			//act
			var actual = _instance.CreateMultiple(new List<TestDbObject> { entity1, entity2 });

			//assert
			actual.Should().NotBeNull();
			actual.Should().Contain(entity1);
			actual.Should().Contain(entity2);

			var entityFromDb = _instance.RetrieveAll();
			entityFromDb.Count.Should().Be(2);
		}

		#endregion

		#region Retrieve

		[Fact]
		public void RetrieveById_WHERE_entity_does_not_exist_SHOULD_return_null()
		{
			//act
			var actual = _instance.RetrieveById(5);

			//assert
			actual.Should().BeNull();
		}

		[Fact]
		public void RetrieveById()
		{
			//arrange
			const int entityId = 345;
			var entity = new TestDbObject { Id = entityId };

			_context.Add(entity);
			_context.SaveChanges();

			//act
			var actual = _instance.RetrieveById(entityId);

			//assert
			actual.Should().Be(entity);
		}

		[Fact]
		public void RetrieveAll_WHERE_no_entities_exist_SHOULD_return_empty_list()
		{
			//act
			var actual = _instance.RetrieveAll();

			//assert
			actual.Should().BeEmpty();
		}

		[Fact]
		public void RetrieveAll()
		{
			//arrange
			const int entity1Id = 677;
			var entity1 = new TestDbObject { Id = entity1Id };

			const int entity2Id = 21;
			var entity2 = new TestDbObject { Id = entity2Id };

			_context.AddRange(new List<TestDbObject> { entity1, entity2 });
			_context.SaveChanges();

			//act
			var actual = _instance.RetrieveAll().ToList();

			//assert
			actual.Count.Should().Be(2);
			actual.Select(x => x.Id).Should().BeEquivalentTo(new List<int> { entity1Id, entity2Id });
		}

		[Fact]
		public void RetrieveByIdWithIncludedMembers()
		{
			//arrange
			const int relatedObjectId = 33;
			var relatedTestObject = new RelatedTestObject { Id = relatedObjectId };

			const int entityId = 677;
			var entity = new TestDbObject { Id = entityId, RelatedObjects = new List<RelatedTestObject> { relatedTestObject } };

			_context.Add(relatedTestObject);
			_context.Add(entity);
			_context.SaveChanges();

			_context.ChangeTracker.Clear();

			//act
			var actual = _instance.RetrieveByIdWithIncludedMembers(x => x.Id == entityId, p => p.RelatedObjects);

			//assert
			actual.Should().NotBeNull();
			actual.Id.Should().Be(entity.Id);
			actual.RelatedObjects.Should().NotBeNull();
			actual.RelatedObjects.First().Id.Should().Be(relatedObjectId);
		}

		#endregion

		#region Update

		[Fact]
		public void Update()
		{
			//arrange
			const int entityId = 5555;
			var entity = new TestDbObject { Id = entityId };

			_context.Add(entity);
			_context.SaveChanges();

			const string newTestString = "testy mctestface";
			entity.TestString = newTestString;

			//act
			var actual = _instance.Update(entity);

			//assert
			actual.TestString.Should().Be(newTestString);

			var entityFromDb = _instance.RetrieveById(entityId);
			entityFromDb.TestString.Should().Be(newTestString);
		}

		[Fact]
		public void UpdateMany()
		{
			//arrange
			const string oldStringValue = "old string";
			var entity1 = new TestDbObject { Id = 45, TestString = oldStringValue, TestBool = false };
			var entity2 = new TestDbObject { Id = 56666, TestString = oldStringValue, TestBool = false };

			var entitiesToUpdate = new List<TestDbObject> { entity1, entity2 };
			_context.AddRange(entitiesToUpdate);
			_context.SaveChanges();

			const string newTestString = "testy mctestface";
			entity1.TestString = newTestString;

			entity2.TestBool = true;

			//act
			var actual = _instance.UpdateMany(entitiesToUpdate);

			//assert
			actual.Count.Should().Be(2);
			actual[0].TestString.Should().Be(newTestString);
			actual[0].TestBool.Should().BeFalse();
			actual[1].TestString.Should().Be(oldStringValue);
			actual[1].TestBool.Should().BeTrue();

			var entityFromDb = _instance.RetrieveAll();
			entityFromDb[0].TestString.Should().Be(newTestString);
			entityFromDb[0].TestBool.Should().BeFalse();
			entityFromDb[1].TestString.Should().Be(oldStringValue);
			entityFromDb[1].TestBool.Should().BeTrue();
		}

		#endregion

		#region Deletes

		[Fact]
		public void Delete()
		{
			//arrange
			const int entityId = 88;
			var entity = new TestDbObject { Id = entityId };

			_context.Add(entity);
			_context.SaveChanges();

			//act
			_instance.Delete(entity);

			//assert
			var entityFromDb = _instance.RetrieveById(entityId);
			entityFromDb.Should().BeNull();
		}

		[Fact]
		public void Delete_List()
		{
			//arrange
			var entity1 = new TestDbObject { Id = 88 };
			var entity2 = new TestDbObject { Id = 8787887 };

			var entities = new List<TestDbObject> { entity1, entity2 };
			_context.AddRange(entities);
			_context.SaveChanges();

			//act
			_instance.Delete(entities);

			//assert
			var entityFromDb = _instance.RetrieveAll();
			entityFromDb.Should().BeEmpty();
		}

		#endregion
	}
}
