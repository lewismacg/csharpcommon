using FluentAssertions;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CSharpCommon.Tests.Shared.FluentValidation
{
	public static class ValidatorExtensionMethods
	{
		public static void ContainsRuleFor<T>(this IValidator validator, Expression<Func<T, object>> propertyExpression)
		{
			var propertyToTest = ((MemberExpression)propertyExpression.Body).Member.Name;
			ContainsRuleFor(validator, propertyToTest);
		}

		public static void ContainsRuleFor(this IValidator validator, string propertyToTest)
		{
			var validationRules = validator.CreateDescriptor().GetValidatorsForMember(propertyToTest).Select(x => x.Validator);
			var listToTest = new List<IPropertyValidator>(validationRules);
			listToTest.Should().NotBeEmpty($"No validation rules have been defined for {propertyToTest}");
		}

		public static void ContainsChildValidatorFor<T>(this IValidator validator, Expression<Func<T, object>> expression, Type type)
		{
			var propertyToTest = ((MemberExpression)expression.Body).Member.Name;
			var validationRules = validator.CreateDescriptor().GetValidatorsForMember(propertyToTest).Select(x => x.Validator);

			var childValidatorsForMember = validationRules.OfType<IChildValidatorAdaptor>().Select(x => x.ValidatorType);
			var validatorTypes = childValidatorsForMember as Type[] ?? childValidatorsForMember.ToArray();

			var result = validatorTypes.Any(x => type.GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()));
			result.Should().BeTrue($"Property '{propertyToTest}' was expected to have a child validator of type {type} but this was not found.");
		}
	}
}
