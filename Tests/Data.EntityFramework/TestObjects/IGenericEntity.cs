using System;

namespace CSharpCommon.Tests.Data.EntityFramework.TestObjects
{
	public interface IGenericEntity<TKey>
	{
		TKey Id { get; set; }
		DateTime? LastModified { get; set; }
	}
}
