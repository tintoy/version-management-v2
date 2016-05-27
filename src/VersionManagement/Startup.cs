using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Linq;

namespace DD.Cloud.VersionManagement
{
    using DataAccess;
    using DataAccess.Models;

    /// <summary>
    ///    Configuration logic for the version-management application.
    /// </summary>
    public sealed class Startup
	{
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
				
			services.AddLogging();

			services.AddDbContext<VersionManagementEntities>(options =>
			{
				options.UseSqlite("Data Source=../VersionManagement2.db");
			});

			services.AddScoped<IVersionManagementData, VersionManagementData>();

			services.AddMvc()
				.AddRazorOptions(razor =>
				{
					
				})
				.AddJsonOptions(json =>
				{
					json.SerializerSettings.Converters.Add(
						new StringEnumConverter()
					);
				});
		}

		/// <summary>
		///   Configure the application pipeline.
		/// </summary>
		/// <param name="app">
		///     The application pipeline builder.
		/// </param>
		/// <param name="loggerFactory">
		///     The application-level logger factory.
		/// </param>
		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			if (app == null)
				throw new ArgumentNullException(nameof(app));

			if (loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory));
			
			loggerFactory.AddConsole(LogLevel.Warning);
			
			app.UseDeveloperExceptionPage();

			// Ensure database is created / upgraded at startup.
			using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				VersionManagementEntities versionManagementEntities = serviceScope.ServiceProvider.GetService<VersionManagementEntities>();
				versionManagementEntities.Database.Migrate();
				CreateInitialData(versionManagementEntities);
			}

			app.UseMvcWithDefaultRoute();
			app.UseStaticFiles();
		}

		/// <summary>
		/// 	Seed the version-management database with initial data, if required.
		/// </summary>
		/// <param name="versionManagementEntities">
		///		The version-management entity context.
		/// </param>
		void CreateInitialData(VersionManagementEntities versionManagementEntities)
		{
			if (versionManagementEntities == null)
				throw new ArgumentNullException(nameof(versionManagementEntities));

			if (versionManagementEntities.Products.Any())
				return;

			VersionRangeData versionRange1 = new VersionRangeData
			{
				Name = "Product1 R1.0",
				StartVersion = new Version("1.0.0.0"),
				EndVersion = new Version("1.0.9999.0"),
				NextVersion = new Version("1.0.0.0"),
				IncrementBy = VersionComponent.Build
			};
			versionManagementEntities.VersionRanges.Add(versionRange1);

			VersionRangeData versionRange2 = new VersionRangeData
			{
				Name = "Product2 R1.5",
				StartVersion = new Version("1.5.0.0"),
				EndVersion = new Version("1.5.9999.0"),
				NextVersion = new Version("1.5.0.0"),
				IncrementBy = VersionComponent.Build
			};
			versionManagementEntities.VersionRanges.Add(versionRange2);

			ProductData product1 = new ProductData
			{
				Name = "Product1",
				Releases =
				{
					new ReleaseData
					{
						Name = "R1",
						VersionRange = versionRange1,
						SpecialVersion = null
					},
					new ReleaseData
					{
						Name = "R1-Develop",
						VersionRange = versionRange1,
						SpecialVersion = "develop"
					}
				}
			};
			versionManagementEntities.Products.Add(product1);

			ProductData product2 = new ProductData
			{
				Name = "Product2",
				Releases =
				{
					new ReleaseData
					{
						Name = "R1.5",
						VersionRange = versionRange2,
						SpecialVersion = null
					},
					new ReleaseData
					{
						Name = "R1.5-Develop",
						VersionRange = versionRange2,
						SpecialVersion = "develop"
					}
				}
			};
			versionManagementEntities.Products.Add(product2);

			versionManagementEntities.SaveChanges();
		}

		/// <summary>
		/// 	The main program entry point.
		/// </summary>
		/// <param name="args">
		///		Command-line arguments.
		/// </param>
		public static void Main(string[] args)
		{
			var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(
					Directory.GetCurrentDirectory()
				)
                .UseStartup<Startup>()
                .Build();

            host.Run();
		}
	}
}
