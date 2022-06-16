using System;
using System.Threading.Tasks;

namespace CSharpCommon.ErrorReporting.Raygun.Services
{
	public interface IErrorReportingService
	{
		public void ReportError(Exception ex);
		public void ReportError(Exception ex, string name);
		public Task ReportErrorAsync(Exception ex);
		public Task ReportErrorAsync(Exception ex, string name);
	}
}
