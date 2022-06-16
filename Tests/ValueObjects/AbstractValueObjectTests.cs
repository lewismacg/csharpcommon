using FluentAssertions;
using CSharpCommon.Tests.Shared.Unit;
using CSharpCommon.Tests.ValueObjects.TestObjects;
using Xunit;

namespace CSharpCommon.Tests.ValueObjects
{
	public class AbstractValueObjectTests : UnitTestBase
	{
		#region Equals

		[Fact]
		public void Equals_WHERE_other_object_is_same_reference_SHOULD_return_true()
		{
			//arrange
			var valueObject = new TestValueObject();

			//act
			var actual = valueObject.Equals(valueObject);

			//assert
			actual.Should().BeTrue();
		}

		[Fact]
		public void Equals_WHERE_other_object_is_null_SHOULD_return_false()
		{
			//arrange
			var valueObject = new TestValueObject();

			//act
			var actual = valueObject.Equals(null);

			//assert
			actual.Should().BeFalse();
		}

		[Fact]
		public void Equals_WHERE_other_object_is_different_type_SHOULD_return_false()
		{
			//arrange
			var valueObject = new TestValueObject();
			var differentValueObject = new AlternateTestValueObject();

			//act
			var actual = valueObject.Equals(differentValueObject);

			//assert
			actual.Should().BeFalse();
		}

		[Fact]
		public void Equals_WHERE_other_object_is_same_type_but_has_different_values_SHOULD_return_false()
		{
			//arrange
			var valueObject = new TestValueObject { IntegerProperty = 31, StringProperty = "test" };
			var secondValueObject = new TestValueObject { IntegerProperty = 9999, StringProperty = "test" };

			//act
			var actual = valueObject.Equals(secondValueObject);

			//assert
			actual.Should().BeFalse();
		}

		[Fact]
		public void Equals_WHERE_other_object_is_same_type_with_same_values_SHOULD_return_true()
		{
			//arrange
			var valueObject = new TestValueObject { IntegerProperty = 10, StringProperty = "test" };
			var secondValueObject = new TestValueObject { IntegerProperty = 10, StringProperty = "test" };

			//act
			var actual = valueObject.Equals(secondValueObject);

			//assert
			actual.Should().BeTrue();
		}

		#endregion

		#region GetHashCode

		[Fact]
		public void GetHashCode_WHERE_two_objects_are_same_reference_SHOULD_return_same_value()
		{
			//arrange
			var valueObject = new TestValueObject();

			//act + assert
			valueObject.GetHashCode().Should().Be(valueObject.GetHashCode());
		}

		[Fact]
		public void GetHashCode_WHERE_two_objects_have_different_values_for_equatable_properties_SHOULD_return_different_values()
		{
			//arrange
			var valueObject = new TestValueObject
			{
				InconsequentialProperty = true,
				IntegerProperty = 1,
				StringProperty = "hello"
			};

			var secondValueObject = new TestValueObject
			{
				InconsequentialProperty = false,
				IntegerProperty = 2,
				StringProperty = "world"
			};

			//act + assert
			valueObject.GetHashCode().Should().NotBe(secondValueObject.GetHashCode());
		}

		[Fact]
		public void GetHashCode_WHERE_two_objects_have_same_values_for_all_Relevant_Atomic_properties_SHOULD_return_same_value()
		{
			//arrange
			const string stringValue = "hello";
			const int integerValue = 42;
			var valueObject = new TestValueObject
			{
				StringProperty = stringValue,
				IntegerProperty = integerValue,
				InconsequentialProperty = true
			};

			var otherValueObject = new TestValueObject
			{
				StringProperty = stringValue,
				IntegerProperty = integerValue,
				InconsequentialProperty = false
			};

			//act + assert
			valueObject.GetHashCode().Should().Be(otherValueObject.GetHashCode());
		}

		#endregion

	}
}
