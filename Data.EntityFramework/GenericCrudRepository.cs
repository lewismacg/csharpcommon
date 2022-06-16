using CSharpCommon.Data.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CSharpCommon.Data.EntityFramework
{
	public class GenericCrudRepository<TEntity, TKey, TDbContext> : ICrudProvider<TEntity, TKey> where TEntity : class where TDbContext : DbContext
	{
		private readonly TDbContext _dbContext;
		protected internal virtual DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();

		#region Constructors

		public GenericCrudRepository(TDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#endregion

		#region Create

		public virtual TEntity Create(TEntity entity)
		{
			DbSet.Add(entity);
			_dbContext.SaveChanges();

			return entity;
		}

		public virtual List<TEntity> CreateMultiple(List<TEntity> entities)
		{
			DbSet.AddRange(entities);
			_dbContext.SaveChanges();

			return entities;
		}

		#endregion

		#region Retrieve

		public virtual TEntity RetrieveById(TKey entityId) => DbSet.Find(entityId);

		public virtual List<TEntity> RetrieveAll() => DbSet.ToList();

		public virtual TEntity RetrieveByIdWithIncludedMembers(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> result = DbSet.AsNoTracking();
			if (includes.Any() == false) return result.FirstOrDefault(predicate);

			result = includes.Aggregate(result, (current, include) => current.Include(include).AsNoTracking());

			return result.AsNoTracking().FirstOrDefault(predicate);
		}

		#endregion

		#region Update

		public virtual TEntity Update(TEntity entity)
		{
			DbSet.Update(entity);
			_dbContext.SaveChanges();

			return entity;
		}

		public virtual List<TEntity> UpdateMany(List<TEntity> entities)
		{
			DbSet.UpdateRange(entities);
			_dbContext.SaveChanges();

			return entities;
		}

		#endregion

		#region Delete

		public virtual void Delete(TEntity entity)
		{
			DbSet.Remove(entity);
			_dbContext.SaveChanges();
		}

		public virtual void Delete(List<TEntity> entitySelector)
		{
			entitySelector.ForEach(x => DbSet.Remove(x));
			_dbContext.SaveChanges();
		}

		#endregion

	}
}
