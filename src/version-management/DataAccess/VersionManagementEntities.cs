using System;
using Microsoft.Data.Entity;

namespace DD.Cloud.VersionManagement.DataAccess
{
    using Models;

    /// <summary>
    ///		Entity context for version-management data.
    /// </summary>
    public sealed class VersionManagementEntities
		: DbContext
	{
		/// <summary>
		///		Create a new version-management entity context. 
		/// </summary>
		public VersionManagementEntities()
		{
		}
		
		/// <summary>
		///		All products in the version-management database. 
		/// </summary>
		public DbSet<Product> Products { get; private set; }
		
		/// <summary>
		///		Called when the data model is being created. 
		/// </summary>
		/// <param name="options">
		///		 The data model builder.
		/// </param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			if (modelBuilder == null)
				throw new ArgumentNullException("modelBuilder");
				
			var products = modelBuilder.Entity<Product>();
			
			products.HasKey(product => product.Id);
			products.Property(product => product.Id)
				.ValueGeneratedOnAdd();
				
			products.Property(product => product.Name)
				.IsRequired()
				.HasMaxLength(30);
		}
	}
}