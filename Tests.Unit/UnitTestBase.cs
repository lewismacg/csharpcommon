using Moq.AutoMock;

namespace CSharpCommon.Tests.Shared.Unit
{
	public class UnitTestBase
	{
		protected readonly AutoMocker _autoMocker = new AutoMocker();

		protected T CreateInstance<T>() where T : class
		{
			return _autoMocker.CreateInstance<T>();
		}
	}
}
