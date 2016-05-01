using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;

namespace DD.Cloud.VersionManagement.FunctionalTests
{
	using Utilities;

	/// <summary>
	///		Extension methods for <see cref="IServiceCollection"/>.
	/// </summary>
	public static class ServiceCollectionExtensions
    {
		/// <summary>
		///		Add a custom <see cref="IApplicationEnvironment"/> service that overrides the application name and base path.
		/// </summary>
		/// <param name="services">
		///		The service collection.
		/// </param>
		/// <param name="applicationName">
		///		The application name.
		/// </param>
		/// <param name="applicationBasePath">
		///		The application base path.
		/// </param>
		/// <returns>
		///		The service collection (enables method chaining).
		/// </returns>
		public static IServiceCollection OverrideApplicationEnvironment(this IServiceCollection services, string applicationName, string applicationBasePath)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			if (String.IsNullOrWhiteSpace(applicationName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'applicationName'.", nameof(applicationName));

			if (String.IsNullOrWhiteSpace(applicationBasePath))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'applicationBasePath'.", nameof(applicationBasePath));

			applicationBasePath = Path.GetFullPath(applicationBasePath);

			// Slightly ugly hack.
			IApplicationEnvironment originalEnvironment = CallContextServiceLocator.Locator.ServiceProvider.GetRequiredService<IApplicationEnvironment>();
			IApplicationEnvironment shimApplicationEnvironment = new TestApplicationEnvironment(originalEnvironment, applicationName, applicationBasePath);

			// Add a shim for the application environment so we point to the correct base path.
			services.Replace(new ServiceDescriptor(
				serviceType: typeof(IApplicationEnvironment),
				instance: shimApplicationEnvironment
			));

			return services;
		}
	}
}
