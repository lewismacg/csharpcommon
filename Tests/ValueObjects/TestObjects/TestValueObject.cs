using System.Collections.Generic;
using CSharpCommon.ValueObjects;

namespace CSharpCommon.Tests.ValueObjects.TestObjects
{
	public class TestValueObject : AbstractValueObject
	{
		public string StringProperty { get; set; }
		public int IntegerProperty { get; set; }
		public bool InconsequentialProperty { get; set; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return StringProperty;
			yield return IntegerProperty;
		}
	}
}
