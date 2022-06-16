using FluentAssertions;
using FluentValidation.Results;

namespace CSharpCommon.Tests.Shared.FluentValidation
{
	public static class ValidationResultExtensionMethods
	{
		public static void AssertValidationFails(this ValidationResult results, string errorMessage)
		{
			results.IsValid.Should().BeFalse();
			results.Errors.Should().NotBeEmpty();
			results.Errors.Should().Contain(x => x.ErrorMessage == errorMessage);
		}

		public static void AssertValidationPasses(this ValidationResult results)
		{
			results.IsValid.Should().BeTrue();
			results.Errors.Should().BeEmpty();
		}

		public static void AssertValidationFails(this ValidationResult results, string fieldName, string errorMessage)
		{
			results.IsValid.Should().BeFalse();
			results.Errors.Should().NotBeEmpty();
			results.Errors.Should().Contain(x => x.ErrorMessage == errorMessage && x.PropertyName == fieldName);
		}
	}
}
