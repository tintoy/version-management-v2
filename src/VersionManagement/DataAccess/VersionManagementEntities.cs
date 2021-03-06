using System;
using Microsoft.EntityFrameworkCore;

namespace DD.Cloud.VersionManagement.DataAccess
{
	using Models;

	/// <summary>
	///		The version-management entity context.
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
		///		Create a new version-management entity context using the specified options.
		/// </summary>
		/// <param name="options">
		///		Create a new version-management entity context.
		/// </param>
		public VersionManagementEntities(DbContextOptions<VersionManagementEntities> options)
			: base(options)
		{
		}

		/// <summary>
		///		The set of all <see cref="ProductData"/> entities.
		/// </summary>
		public DbSet<ProductData> Products { get; set; }

		/// <summary>
		///		The set of all <see cref="ReleaseData"/> entities.
		/// </summary>
		public DbSet<ReleaseData> Releases { get; set; }

		/// <summary>
		///		The set of all <see cref="ReleaseVersionData"/> entities.
		/// </summary>
		public DbSet<ReleaseVersionData> ReleaseVersions { get; set; }

		/// <summary>
		///		The set of all <see cref="VersionRangeData"/> entities.
		/// </summary>
		public DbSet<VersionRangeData> VersionRanges { get; set; }

		/// <summary>
		///		Called when the entity model is being created.
		/// </summary>
		/// <param name="modelBuilder">
		///		The entity model builder.
		/// </param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			if (modelBuilder == null)
				throw new ArgumentNullException(nameof(modelBuilder));

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ProductData>()
				.HasKey(product => product.Id);

			modelBuilder.Entity<ReleaseData>()
				.HasKey(release => release.Id);

			modelBuilder.Entity<VersionRangeData>()
				.HasKey(versionRange => versionRange.Id);

			modelBuilder.Entity<ReleaseVersionData>()
				.HasKey(releaseVersion => new
				{
					releaseVersion.CommitId,
					releaseVersion.ReleaseId
				});
		}
	}
}
