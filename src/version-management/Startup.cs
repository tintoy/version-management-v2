using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using System;
using DD.Cloud.VersionManagement.DataAccess;

namespace DD.Cloud.VersionManagement
{
	/// <summary>
	///		Configuration for the version-management application.
	/// </summary>
    public sealed class Startup
    {
        /// <summary>
		///		Add and configure services in the application container.
		/// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
			if (services == null)
				throw new ArgumentNullException("services");
				
			services.AddEntityFramework()
				.AddSqlite()
				.AddDbContext<VersionManagementEntities>(options => {
					options.UseSqlite(
						connectionString: "Data Source=VersionManagement2.db"
					);
				});
				
			services.AddMvc()
				.AddJsonOptions(json => {
					json.SerializerSettings.Converters.Add(
						new StringEnumConverter()
					);
				}); 
        }

        /// <summary>
		///		Configure the HTTP request pipeline.
		/// </summary>
        public void Configure(IApplicationBuilder app)
        {
            if (app == null)
				throw new ArgumentNullException("app");

            app.UseMvcWithDefaultRoute();
        }

        /// <summary>
		///		The main program entry-point.
		/// </summary>
		/// <param name="args">
		///		Command-line arguments.
		/// </param>
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
