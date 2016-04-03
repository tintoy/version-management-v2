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
        ///     The application pipeline builder for end-to-end tests.
        /// </summary>
        readonly WebHostBuilder TestAppBuilder = TestServer.CreateBuilder().UseStartup<Startup>(); // AF: Can we then override service registrations to mock out things like data access?
        
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
    }
}
