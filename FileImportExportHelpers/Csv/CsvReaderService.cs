using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpCommon.FileImportExportHelpers.Csv.Interfaces;

namespace CSharpCommon.FileImportExportHelpers.Csv
{
	public class CsvReaderService<T, TClassMap> : ICsvReaderService<T> where T : class where TClassMap : ClassMap
	{
		public async Task<List<T>> ReadCsv(Stream file)
		{
			var records = new List<T>();
			using (var reader = new StreamReader(file))
			{
				using (var csvReader = new CsvReader(reader, CultureInfo.GetCultureInfo("en-GB")))
				{
					csvReader.Context.RegisterClassMap<TClassMap>();
					await csvReader.ReadAsync();
					csvReader.ReadHeader();
					while (await csvReader.ReadAsync())
					{
						var record = csvReader.GetRecord<T>();
						records.Add(record);
					}
				}
			}

			return records;
		}

		public async Task<List<T>> ReadExcel(Stream fileLocation)
		{
			var records = new List<T>();
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			byte[] bytes;

			await using (var stream = new MemoryStream())
			{
				await fileLocation.CopyToAsync(stream);
				stream.Position = 0;
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					var spreadsheet = reader.AsDataSet();
					var table = spreadsheet.Tables[0];
					var csv = ConvertExcelToCsv(table);
					bytes = Encoding.UTF8.GetBytes(csv);
				}
			}

			try
			{
				using var reader = new StreamReader(new MemoryStream(bytes));
				using (var csvReader = new CsvReader(reader, CultureInfo.GetCultureInfo("en-GB")))
				{
					csvReader.Context.RegisterClassMap<TClassMap>();
					csvReader.Read();
					csvReader.ReadHeader();
					while (csvReader.Read())
					{
						var record = csvReader.GetRecord<T>();
						records.Add(record);
					}
				}
			}
			catch (Exception)
			{
				return new List<T>();
			}

			return records;
		}

		private string ConvertExcelToCsv(DataTable dt)
		{
			var sb = new StringBuilder();
			foreach (DataRow row in dt.Rows) sb.AppendLine(string.Join(",", row.ItemArray.Select(field => field?.ToString())));

			return sb.ToString();
		}
	}
}
