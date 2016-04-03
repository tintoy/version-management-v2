using Xunit;

namespace DD.Cloud.VersionManagement.Tests
{
	/// <summary>
	///		Some example unit tests.
	/// </summary>
	public class ExampleTests
    {
		/// <summary>
		///		A test that passes.
		/// </summary>
		[Fact]
		public void This_Test_Passes()
		{
			Assert.Equal(true, true);
			Assert.Equal(false, false);
			Assert.NotEqual(true, false);
			Assert.NotEqual(false, true);
		}
    }
}
