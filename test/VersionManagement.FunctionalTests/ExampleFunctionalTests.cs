using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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
		///     The application pipeline builder for end-to-end tests.
		/// </summary>
		readonly IWebHostBuilder TestAppBuilder = new WebHostBuilder().UseStartup<TestStartup>();

		/// <summary>
		///		Create a new <see cref="ExampleFunctionalTests"/>.
		/// </summary>
		public ExampleFunctionalTests()
		{
			// Override the application base path so that views are correctly resolved.
			TestStartup.AppBasePath = "src/VersionManagement";
		}
		
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
			{
				using (HttpResponseMessage response = await testServer.CreateRequest("/").GetAsync())
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
			{
				using (HttpResponseMessage response = await testServer.CreateRequest("/products").GetAsync())
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
