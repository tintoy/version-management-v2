using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;

namespace DD.Cloud.VersionManagement
{
	using DataAccess;
	using DataAccess.Models;
	using Microsoft.AspNet.Diagnostics;

	public sealed class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			services.AddEntityFramework()
				.AddSqlite()
				.AddDbContext<VersionManagementEntities>(options =>
				{
					options.UseSqlite(
						connectionString: "Data Source=../VersionManagement2.db"
					);
				});

			services.AddMvc()
				.AddJsonOptions(json =>
				{
					json.SerializerSettings.Converters.Add(
						new StringEnumConverter()
					);
				});
		}

		public void Configure(IApplicationBuilder app)
		{
			if (app == null)
				throw new ArgumentNullException(nameof(app));

			app.Properties["host.AppMode"] = "development";
			app.UseDeveloperExceptionPage();

			// Ensure database is created / upgraded at startup.
			using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				VersionManagementEntities versionManagementEntities = serviceScope.ServiceProvider.GetService<VersionManagementEntities>();
				versionManagementEntities.Database.Migrate();
				SeedDatabase(versionManagementEntities);
			}

			app.UseMvcWithDefaultRoute();
			app.UseStaticFiles();
		}

		void SeedDatabase(VersionManagementEntities versionManagementEntities)
		{
			if (versionManagementEntities == null)
				throw new ArgumentNullException(nameof(versionManagementEntities));

			if (versionManagementEntities.Products.Any())
				return;

			VersionRange versionRange1 = new VersionRange
			{
				Name = "Product1",
				StartVersion = new Version("1.0.0.0"),
				EndVersion = new Version("1.0.9999.0"),
				NextVersion = new Version("1.0.0.0"),
				IncrementBy = VersionComponent.Build
			};
			versionManagementEntities.VersionRanges.Add(versionRange1);

			VersionRange versionRange2 = new VersionRange
			{
				Name = "Product2",
				StartVersion = new Version("1.5.0.0"),
				EndVersion = new Version("1.5.9999.0"),
				NextVersion = new Version("1.5.0.0"),
				IncrementBy = VersionComponent.Build
			};
			versionManagementEntities.VersionRanges.Add(versionRange2);

			Product product1 = new Product
			{
				Name = "Product1",
				Releases =
				{
					new Release
					{
						Name = "R1",
						VersionRange = versionRange1,
						SpecialVersion = null
					},
					new Release
					{
						Name = "R1-Develop",
						VersionRange = versionRange1,
						SpecialVersion = "develop"
					}
				}
			};
			versionManagementEntities.Products.Add(product1);

			Product product2 = new Product
			{
				Name = "Product2",
				Releases =
				{
					new Release
					{
						Name = "R1.5",
						VersionRange = versionRange2,
						SpecialVersion = null
					},
					new Release
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

		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
	}
}
