using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CSharpCommon.FileImportExportHelpers.Csv.Interfaces
{
	public interface ICsvReaderService<T> where T : class
	{
		Task<List<T>> ReadCsv(Stream file);
		Task<List<T>> ReadExcel(Stream fileLocation);
	}
}
