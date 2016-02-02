using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using System;
using DD.Cloud.VersionManagement.DataAccess;

namespace DD.Cloud.VersionManagement
{
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

			// Ensure database is created / upgraded at startup.
			using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				serviceScope.ServiceProvider.GetService<VersionManagementEntities>()
					.Database.Migrate();
			}

			app.UseMvcWithDefaultRoute();
		}

		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
	}
}
