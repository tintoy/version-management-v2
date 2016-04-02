using System;
using System.Collections.Generic;
using System.Linq;
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
		///		Get the <see cref="ReleaseVersion"/> (if it exists) for the specified combination of product, release, and commit Id.
		/// </summary>
		/// <param name="productName">
		///		The product name.
		/// </param>
		/// <param name="releaseName">
		///		The release name.
		/// </param>
		/// <param name="commitId">
		///		The commit Id.
		/// </param>
		/// <returns>
		///		The <see cref="ReleaseVersion"/>, or <c>null</c> if no matching <see cref="ReleaseVersion"/> was found.
		/// </returns>
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

		/// <summary>
		///		Get or create the <see cref="ReleaseVersion"/> for the specified combination of product, release, and commit Id.
		/// </summary>
		/// <param name="productName">
		///		The product name.
		/// </param>
		/// <param name="releaseName">
		///		The release name.
		/// </param>
		/// <param name="commitId">
		///		The commit Id.
		/// </param>
		/// <returns>
		///		The <see cref="ReleaseVersion"/>.
		/// </returns>
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

		/// <summary>
		///		Get the <see cref="ReleaseVersion"/>(s) for the specified combination of product and commit Id.
		/// </summary>
		/// <param name="productName">
		///		The product name.
		/// </param>
		/// <param name="commitId">
		///		The commit Id.
		/// </param>
		/// <returns>
		///		A read-only list of matching <see cref="ReleaseVersion"/>s.
		/// </returns>
		public IReadOnlyList<ReleaseVersion> GetReleaseVersionsFromCommitId(string productName, string commitId)
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

		/// <summary>
		///		Get the <see cref="ReleaseVersion"/>(s) for the specified combination of product and semantic version.
		/// </summary>
		/// <param name="productName">
		///		The product name.
		/// </param>
		/// <param name="semanticVersion">
		///		The semantic version.
		/// </param>
		/// <returns>
		///		A read-only list of matching <see cref="ReleaseVersion"/>s.
		/// </returns>
		public IReadOnlyList<ReleaseVersion> GetReleaseVersionsFromSemanticVersion(string productName, string semanticVersion)
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
