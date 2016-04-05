using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Runtime.Versioning;

namespace DD.Cloud.VersionManagement.FunctionalTests.Utilities
{
	/// <summary>
	///		An <see cref="IApplicationEnvironment"/> shim for use in tests.
	/// </summary>
	/// <remarks>
	///		Used to override the application name and base path.
	/// </remarks>
	public class TestApplicationEnvironment
		: IApplicationEnvironment
	{
		/// <summary>
		///		Create a new <see cref="TestApplicationEnvironment"/>.
		/// </summary>
		/// <param name="originalEnvironment">
		///		The original <see cref="IApplicationEnvironment"/>.
		/// </param>
		/// <param name="applicationName">
		///		The application name.
		/// </param>
		/// <param name="basePath">
		///		The application base path.
		/// </param>
		public TestApplicationEnvironment(IApplicationEnvironment originalEnvironment, string applicationName, string basePath)
		{
			if (originalEnvironment == null)
				throw new ArgumentNullException(nameof(originalEnvironment));

			if (String.IsNullOrWhiteSpace(applicationName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'applicationName'.", nameof(applicationName));

			if (String.IsNullOrWhiteSpace(basePath))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'basePath'.", nameof(basePath));

			OriginalEnvironment = originalEnvironment;
			ApplicationName = applicationName;
			ApplicationBasePath = basePath;
		}

		IApplicationEnvironment OriginalEnvironment { get; }

		/// <summary>
		///		The current application name.
		/// </summary>
		public string ApplicationName { get; }

		/// <summary>
		///		The application version.
		/// </summary>
		public string ApplicationVersion => OriginalEnvironment.ApplicationVersion;

		/// <summary>
		///		The application base path.
		/// </summary>
		public string ApplicationBasePath { get; }

		/// <summary>
		///		The runtime framework used by the application.
		/// </summary>
		public FrameworkName RuntimeFramework => OriginalEnvironment.RuntimeFramework;

		/// <summary>
		///     Get the specified value from Application Global Data.
		/// </summary>
		/// <param name="name">
		///     The name of the Application Global Data item to retrieve.
		/// </param>
		/// <returns>
		///     The value of the item identified by <paramref name="name" />, or null if no item exists.
		/// </returns>
		public object GetData(string name) => OriginalEnvironment.GetData(name);

		/// <summary>
		///     Set the specified value in Application Global Data.
		/// </summary>
		/// <param name="name">
		///     The name of the Application Global Data item to set.
		/// </param>
		/// <param name="value">
		///     The value to store in Application Global Data.
		/// </param>
		public void SetData(string name, object value) => OriginalEnvironment.SetData(name, value);

		/// <summary>
		///     Get the application configuration.
		/// </summary>
		/// <remarks>
		///		This should only be used for runtime compilation.
		/// </remarks>
		public string Configuration => OriginalEnvironment.Configuration;
	}
}