using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DD.Cloud.VersionManagement.FunctionalTests
{
	using Utilities;

	/// <summary>
	///		Some example functional tests.
	/// </summary>
	public class ExampleFunctionalTests
	{
		/// <summary>
		///     The application pipeline builder for end-to-end tests.
		/// </summary>
		readonly WebHostBuilder TestAppBuilder =
			TestServer
				.CreateBuilder()
				.UseServices(services =>
				{
					// Slightly ugly hack.
					IApplicationEnvironment originalEnvironment = CallContextServiceLocator.Locator.ServiceProvider.GetRequiredService<IApplicationEnvironment>();

					// Shim the application environment so we point to the correct base path.
					services.AddInstance(typeof(IApplicationEnvironment),
						new TestApplicationEnvironment(originalEnvironment,
							applicationName: "VersionManagement",
							basePath: Path.GetFullPath(@"..\..\src\VersionManagement")
						)
					);
				})
				.UseStartup<Startup>();
		
		/// <summary>
		///		Verify that the home page can be retrieved.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[Fact]
		public async Task Can_Get_Home_Page()
		{
			using (TestServer testServer = new TestServer(TestAppBuilder))
			using (HttpClient testClient = testServer.CreateClient())
			{
				using (HttpResponseMessage response = await testClient.GetAsync("/"))
				{
					response.EnsureSuccessStatusCode();
					
					Assert.Equal("text/plain",
						response.Content.Headers.ContentType.MediaType
					);
				}
			}
		}

		/// <summary>
		///		Verify that the product listing page can be retrieved.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[Fact]
		public async Task Can_Get_Products_Page()
		{
			using (TestServer testServer = new TestServer(TestAppBuilder))
			using (HttpClient testClient = testServer.CreateClient())
			{
				using (HttpResponseMessage response = await testClient.GetAsync("/products"))
				{
					response.EnsureSuccessStatusCode();

					Assert.Equal("text/html",
						response.Content.Headers.ContentType.MediaType
					);
				}
			}
		}
	}
}
