using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace DD.Cloud.VersionManagement.DataAccess
{
	using Models;

	public sealed class VersionManagementEntities
		: DbContext
	{
		public VersionManagementEntities()
		{
		}

		public DbSet<Product> Products { get; set; }

		public DbSet<Release> Releases { get; set; }

		public DbSet<BuildVersion> BuildVersions { get; set; }

		public DbSet<VersionRange> VersionRanges { get; set; }

		public BuildVersion GetBuildVersion(string commitId, string productName, string releaseName)
		{
			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			if (String.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'productName'.", nameof(productName));

			if (String.IsNullOrWhiteSpace(releaseName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'releaseName'.", nameof(releaseName));

			Product matchingProduct = Products.FirstOrDefault(product =>
				product.Name == productName
			);
			if (matchingProduct == null)
				throw new VersionManagementException("Product not found: '{0}'.", productName);

			Release matchingRelease = Releases.FirstOrDefault(release =>
				release.ProductId == matchingProduct.Id
				&&
				release.Name == releaseName
			);
			if (matchingRelease == null)
				throw new VersionManagementException("Release not found: '{0}'.", releaseName);

			BuildVersion matchingVersion = BuildVersions.FirstOrDefault(version =>
				version.ReleaseId == matchingRelease.Id
				&&
				version.CommitId == commitId
			);

			return matchingVersion;
		}

		public BuildVersion GetOrCreateBuildVersion(string commitId, string productName, string releaseName)
		{
			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			if (String.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'productName'.", nameof(productName));

			if (String.IsNullOrWhiteSpace(releaseName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'releaseName'.", nameof(releaseName));

			Product matchingProduct = Products.FirstOrDefault(product =>
				product.Name == productName
			);
			if (matchingProduct == null)
				throw new VersionManagementException("Product not found: '{0}'.", productName);

			Release matchingRelease = Releases.FirstOrDefault(release =>
				release.ProductId == matchingProduct.Id
				&&
				release.Name == releaseName
			);
			if (matchingRelease == null)
				throw new VersionManagementException("Release not found: '{0}'.", releaseName);

			BuildVersion matchingVersion = BuildVersions.FirstOrDefault(version =>
				version.ReleaseId == matchingRelease.Id
				&&
				version.CommitId == commitId
			);
			if (matchingVersion != null)
				return matchingVersion;

			BuildVersion newVersion = matchingRelease.AllocateBuildVersion(commitId);
			BuildVersions.Add(newVersion);

			SaveChanges();

			return newVersion;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			if (modelBuilder == null)
				throw new ArgumentNullException(nameof(modelBuilder));

			base.OnModelCreating(modelBuilder);

			var productEntity = modelBuilder.Entity<Product>();
			productEntity.HasKey(product => product.Id);

			var releaseEntity = modelBuilder.Entity<Release>();
			releaseEntity.HasKey(release => release.Id);

			var versionRangeEntity = modelBuilder.Entity<VersionRange>();
			versionRangeEntity.HasKey(versionRange => versionRange.Id);

			var buildVersionEntity = modelBuilder.Entity<BuildVersion>();
			buildVersionEntity.HasKey(buildVersion => new
			{
				buildVersion.CommitId,
				buildVersion.ReleaseId
			});
		}
	}
}
