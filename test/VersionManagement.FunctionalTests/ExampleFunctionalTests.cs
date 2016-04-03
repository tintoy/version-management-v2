using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DD.Cloud.VersionManagement.FunctionalTests
{
	/// <summary>
	///		Some example functional tests.
	/// </summary>
	public class ExampleFunctionalTests
    {
		/// <summary>
		///		Verify that the version management application can be started.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[Fact]
		public async Task Can_Start_Application()
		{
			WebHostBuilder appBuilder =
				TestServer.CreateBuilder()
					.UseStartup<Startup>();

			using (TestServer testServer = new TestServer(appBuilder))
			using (HttpClient testClient = testServer.CreateClient())
			{
				using (HttpResponseMessage response = await testClient.GetAsync("/"))
				{
					response.EnsureSuccessStatusCode();
				}
			}
		}
    }
}
