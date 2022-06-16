using System;
using System.Collections.Generic;

namespace CSharpCommon.Tests.Data.EntityFramework.TestObjects
{
	public class TestDbObject : IGenericEntity<int>
	{
		public virtual int Id { get; set; }
		public virtual DateTime? LastModified { get; set; }
		public virtual string TestString { get; set; }
		public virtual bool TestBool { get; set; }
		public virtual List<RelatedTestObject> RelatedObjects { get; set; }
	}

	public class RelatedTestObject
	{
		public virtual int Id { get; set; }
		public virtual TestDbObject ParentObject { get; set; }
	}
}
