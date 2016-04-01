using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace DD.Cloud.VersionManagement.DataAccess
{
	using System.Collections.Generic;
	using Models;

	public sealed class VersionManagementEntities
		: DbContext
	{
		public VersionManagementEntities()
		{
		}

		public DbSet<Product> Products { get; set; }

		public DbSet<Release> Releases { get; set; }

		public DbSet<ReleaseVersion> ReleaseVersions { get; set; }

		public DbSet<VersionRange> VersionRanges { get; set; }

		public ReleaseVersion GetReleaseVersion(string productName, string releaseName, string commitId)
		{
			if (String.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'productName'.", nameof(productName));

			if (String.IsNullOrWhiteSpace(releaseName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'releaseName'.", nameof(releaseName));

			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

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

			ReleaseVersion matchingVersion = ReleaseVersions.FirstOrDefault(version =>
				version.ReleaseId == matchingRelease.Id
				&&
				version.CommitId == commitId
			);

			return matchingVersion;
		}

		public ReleaseVersion GetOrCreateReleaseVersion(string productName, string releaseName, string commitId)
		{
			if (String.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'productName'.", nameof(productName));

			if (String.IsNullOrWhiteSpace(releaseName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'releaseName'.", nameof(releaseName));

			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			Product matchingProduct = Products.FirstOrDefault(product =>
				product.Name == productName
			);
			if (matchingProduct == null)
				throw new VersionManagementException("Product not found: '{0}'.", productName);

			Release matchingRelease =
				Releases.Include(release => release.VersionRange)
					.FirstOrDefault(release =>
						release.ProductId == matchingProduct.Id
						&&
						release.Name == releaseName
					);
			if (matchingRelease == null)
				throw new VersionManagementException("Release not found: '{0}'.", releaseName);

			ReleaseVersion matchingVersion = ReleaseVersions.FirstOrDefault(version =>
				version.ReleaseId == matchingRelease.Id
				&&
				version.CommitId == commitId
			);
			if (matchingVersion != null)
				return matchingVersion;

			ReleaseVersion newVersion = matchingRelease.AllocateReleaseVersion(commitId);
			ReleaseVersions.Add(newVersion);

			SaveChanges();

			return newVersion;
		}

		public ReleaseVersion[] GetProductVersionsFromCommitId(string productName, string commitId)
		{
			if (String.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'productName'.", nameof(productName));

			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			return
				ReleaseVersions.Where(buildVersion =>
					buildVersion.Release.Product.Name == productName
					&&
					buildVersion.CommitId == commitId
				)
				.ToArray();
		}

		public ReleaseVersion[] GetProductVersionsFromSemanticVersion(string productName, string semanticVersion)
		{
			if (String.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'productName'.", nameof(productName));

			if (String.IsNullOrWhiteSpace(semanticVersion))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'semanticVersion'.", nameof(semanticVersion));

			string[] versionComponents = semanticVersion.Split('-');
			if (versionComponents.Length > 2)
				throw new FormatException($"Invalid semantic version: '{semanticVersion}'.");

			Version version = new Version(versionComponents[0]);
			string specialVersion = versionComponents.Length == 2 ? versionComponents[1] : String.Empty;

			return
				ReleaseVersions.Where(buildVersion =>
					buildVersion.Release.Product.Name == productName
					&&
					buildVersion.VersionMajor == version.Major
					&&
					buildVersion.VersionMinor == version.Minor
					&&
					buildVersion.VersionBuild == version.Build
					&&
					buildVersion.VersionRevision == version.Revision
					&&
					buildVersion.SpecialVersion == specialVersion
				)
				.ToArray();
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

			var releaseVersionEntity = modelBuilder.Entity<ReleaseVersion>();
			releaseVersionEntity.HasKey(releaseVersion => new
			{
				releaseVersion.CommitId,
				releaseVersion.ReleaseId
			});
		}
	}
}
