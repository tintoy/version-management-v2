using System;
using Microsoft.Data.Entity;

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
		///		The set of all <see cref="Product"/> entities.
		/// </summary>
		public DbSet<Product> Products { get; set; }

		/// <summary>
		///		The set of all <see cref="Release"/> entities.
		/// </summary>
		public DbSet<Release> Releases { get; set; }

		/// <summary>
		///		The set of all <see cref="ReleaseVersion"/> entities.
		/// </summary>
		public DbSet<ReleaseVersion> ReleaseVersions { get; set; }

		/// <summary>
		///		The set of all <see cref="VersionRange"/> entities.
		/// </summary>
		public DbSet<VersionRange> VersionRanges { get; set; }

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

			modelBuilder.Entity<Product>()
				.HasKey(product => product.Id);

			modelBuilder.Entity<Release>()
				.HasKey(release => release.Id);

			modelBuilder.Entity<VersionRange>()
				.HasKey(versionRange => versionRange.Id);

			modelBuilder.Entity<ReleaseVersion>()
				.HasKey(releaseVersion => new
				{
					releaseVersion.CommitId,
					releaseVersion.ReleaseId
				});
		}
	}
}
