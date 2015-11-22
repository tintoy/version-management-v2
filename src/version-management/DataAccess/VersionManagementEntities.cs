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
		///		All releases in the version-management database. 
		/// </summary>
		public DbSet<Release> Releases { get; private set; }
		
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
				
			// Product
			var productEntity = modelBuilder.Entity<Product>();
			
			productEntity.HasKey(product => product.Id);
			productEntity.Property(product => product.Id)
				.ValueGeneratedOnAdd();
				
			productEntity.Property(product => product.Name)
				.IsRequired()
				.HasMaxLength(30);
				
			productEntity.HasMany(product => product.Releases)
				.WithOne(release => release.Product)
				.HasForeignKey(release => release.ProductId);
			
			// Release
			var releaseEntity = modelBuilder.Entity<Release>();
			
			releaseEntity.HasKey(release => release.Id);
			releaseEntity.Property(release => release.Id)
				.ValueGeneratedOnAdd();
				
			releaseEntity.Property(release => release.Name)
				.IsRequired()
				.HasMaxLength(30);
				
			releaseEntity.HasOne(release => release.Product)
				.WithMany(product => product.Releases);
		}
	}
}