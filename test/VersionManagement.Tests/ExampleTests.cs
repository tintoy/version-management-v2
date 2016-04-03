using Xunit;
using Xunit.Abstractions;

namespace DD.Cloud.VersionManagement.Tests
{
	/// <summary>
	///		Some example unit tests.
	/// </summary>
	public class ExampleTests
    {
		/// <summary>
		///		Create a new example unit-test suite.
		/// </summary>
		/// <param name="testOutput">
		///		The unit test output facility.
		/// </param>
		public ExampleTests(ITestOutputHelper testOutput)
		{
			if (testOutput == null)
				throw new System.ArgumentNullException(nameof(testOutput));

			TestOutput = testOutput;
		}

		/// <summary>
		///		The unit test output facility.
		/// </summary>
		ITestOutputHelper TestOutput { get; }

		/// <summary>
		///		A test that passes.
		/// </summary>
		[Fact]
		public void This_Test_Passes()
		{
			TestOutput.WriteLine("This test should pass...");

			Assert.Equal(true, true);
			Assert.Equal(false, false);
			Assert.NotEqual(true, false);
			Assert.NotEqual(false, true);

			TestOutput.WriteLine("This test passed.");
		}
    }
}
