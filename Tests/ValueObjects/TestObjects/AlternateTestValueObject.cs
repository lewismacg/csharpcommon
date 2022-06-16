using System.Collections.Generic;
using CSharpCommon.ValueObjects;

namespace CSharpCommon.Tests.ValueObjects.TestObjects
{
	public class AlternateTestValueObject : AbstractValueObject
	{
		public string StringProperty { get; set; }
		public double DoubleProperty { get; set; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return StringProperty;
			yield return DoubleProperty;
		}
	}
}
