using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using System;
using CSharpCommon.Serialisation.Converters;
using CSharpCommon.Tests.Shared.Unit;
using Xunit;

namespace CSharpCommon.Tests.Serialisation
{
	public class TrimmingConverterTests : UnitTestBase
	{
		private readonly TrimmingConverter _instance;
		private readonly Mock<JsonWriter> _jsonWriter;

		public TrimmingConverterTests()
		{
			_jsonWriter = MoqHelpers.GeneratePartialMock<JsonWriter>();
			_instance = new TrimmingConverter();
		}

		#region CanConvert

		[Theory]
		[InlineData(typeof(int))]
		[InlineData(typeof(bool))]
		[InlineData(typeof(DateTime))]
		public void CanConvert_WHERE_not_type_of_string_SHOULD_return_false(Type objectType)
		{
			//act
			var actual = _instance.CanConvert(objectType);

			//assert
			actual.Should().BeFalse();
		}

		[Fact]
		public void CanConvert_WHERE_string_SHOULD_return_true()
		{
			//act
			var actual = _instance.CanConvert(typeof(string));

			//assert
			actual.Should().BeTrue();
		}

		#endregion

		#region WriteJson

		[Fact]
		public void WriteJson_WHERE_token_is_null_SHOULD_write_null()
		{
			//act
			_instance.WriteJson(_jsonWriter.Object, null, It.IsAny<JsonSerializer>());

			//assert
			_jsonWriter.Verify(x => x.WriteNull(), Times.Once);
		}

		[Fact]
		public void WriteJson_WHERE_token_is_string_SHOULD_convert_and_trim()
		{
			//arrange
			const string value = "  a string   ";

			//act
			_instance.WriteJson(_jsonWriter.Object, value, It.IsAny<JsonSerializer>());

			//assert
			_jsonWriter.Verify(x => x.WriteValue(value.Trim()), Times.Once);
		}

		#endregion
	}
}
