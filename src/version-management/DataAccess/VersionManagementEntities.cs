using System;
using Microsoft.Data.Entity;

namespace DD.Cloud.VersionManagement.DataAccess
{
	using Microsoft.Data.Entity.Metadata.Internal;
	using Models;

    public sealed class VersionManagementEntities
		: DbContext
	{
		public VersionManagementEntities()
		{
		}
		
		public DbSet<Product> Products { get; set; }
		
		public DbSet<Release> Releases { get; set; }
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			if (modelBuilder == null)
				throw new ArgumentNullException(nameof(modelBuilder));
				
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