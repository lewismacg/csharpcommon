using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSharpCommon.Data.EntityFramework.Interfaces
{
	public interface ICrudProvider<TEntity, TKey> where TEntity : class
	{
		TEntity Create(TEntity entity);
		List<TEntity> CreateMultiple(List<TEntity> entities);
		TEntity RetrieveById(TKey entityId);
		TEntity Update(TEntity entity);
		List<TEntity> UpdateMany(List<TEntity> entities);
		void Delete(TEntity entity);
		void Delete(List<TEntity> entitySelector);
		List<TEntity> RetrieveAll();
		TEntity RetrieveByIdWithIncludedMembers(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
	}
}
