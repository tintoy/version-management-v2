using Microsoft.AspNet.Builder;
using Microsoft.AspNet.FileProviders;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using System;

namespace DD.Cloud.VersionManagement.FunctionalTests
{
	/// <summary>
	///		Customised startup logic for use in functional tests.
	/// </summary>
	public sealed class TestStartup
    {
		/// <summary>
		///		The overridden application base path.
		/// </summary>
		public static string AppBasePath { get; set; }

		/// <summary>
		///		The startup logic for the core version management application.
		/// </summary>
		readonly Startup	_appStartup = new Startup();

		/// <summary>
		///		Create a new <see cref="TestStartup"/>.
		/// </summary>
		public TestStartup()
		{
			if (String.IsNullOrWhiteSpace(AppBasePath))
				throw new InvalidOperationException("AppBasePath property has not been set.");
		}

		/// <summary>
		///   Configure / register components and services.
		/// </summary>
		/// <param name="services">
		///     The global service collection.
		/// </param>
		public void ConfigureServices(IServiceCollection services)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			services.OverrideApplicationEnvironment("VersionManagement", AppBasePath);

			_appStartup.ConfigureServices(services);
		}

		/// <summary>
		///   Configure the application pipeline.
		/// </summary>
		/// <param name="app">
		///     The application pipeline builder.
		/// </param>
		/// <param name="environment">
		///		The current application environment.
		/// </param>
		/// <param name="loggerFactory">
		///     The application-level logger factory.
		/// </param>
		/// <param name="razorOptions">
		///		The view options for the Razor view engine.
		/// </param>
		public void Configure(IApplicationBuilder app, IApplicationEnvironment environment, ILoggerFactory loggerFactory, IOptions<RazorViewEngineOptions> razorOptions)
		{
			if (app == null)
				throw new ArgumentNullException(nameof(app));

			if (razorOptions == null)
				throw new ArgumentNullException(nameof(razorOptions));

			// Ensure Razor uses overridden base path.
			razorOptions.Value.FileProvider = new PhysicalFileProvider(environment.ApplicationBasePath);

			_appStartup.Configure(app, loggerFactory);
		}
	}
}
