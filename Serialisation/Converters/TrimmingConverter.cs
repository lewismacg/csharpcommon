using Newtonsoft.Json;
using System;

namespace CSharpCommon.Serialisation.Converters
{
	public class TrimmingConverter : JsonConverter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="objectType"></param>
		/// <returns></returns>
		public override bool CanConvert(Type objectType) => objectType == typeof(string);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="objectType"></param>
		/// <param name="existingValue"></param>
		/// <param name="serializer"></param>
		/// <returns></returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.String) return reader.Value;
			return (reader.Value as string)?.Trim() ?? reader.Value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="value"></param>
		/// <param name="serializer"></param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var text = (string)value;

			if (text == null) writer.WriteNull();
			else writer.WriteValue(text.Trim());
		}
	}
}
