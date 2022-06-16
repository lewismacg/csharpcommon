using Mindscape.Raygun4Net.AspNetCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpCommon.ErrorReporting.Raygun.Configuration;

namespace CSharpCommon.ErrorReporting.Raygun.Services
{
	public class ErrorReportingService : IErrorReportingService
	{
		private readonly IRaygunConfiguration _raygunConfiguration;

		public ErrorReportingService(IRaygunConfiguration raygunConfiguration)
		{
			_raygunConfiguration = raygunConfiguration;
		}

		public void ReportError(Exception ex) => new RaygunClient(_raygunConfiguration.ApiKey).Send(ex);
		public void ReportError(Exception ex, string name) => new RaygunClient(_raygunConfiguration.ApiKey).Send(ex, new List<string> { name });

		public async Task ReportErrorAsync(Exception ex) => await new RaygunClient(_raygunConfiguration.ApiKey).SendInBackground(ex);
		public async Task ReportErrorAsync(Exception ex, string name) => await new RaygunClient(_raygunConfiguration.ApiKey).SendInBackground(ex, new List<string> { name });
	}
}
