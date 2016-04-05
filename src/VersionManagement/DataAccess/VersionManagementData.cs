using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DD.Cloud.VersionManagement.DataAccess
{
	using Models;

	/// <summary>
	///		The version-management data access facility.
	/// </summary>
	public sealed class VersionManagementData
		: IVersionManagementData, IDisposable
	{
		/// <summary>
		///		The version management entity context.
		/// </summary>
		readonly VersionManagementEntities _entityContext;

		/// <summary>
		///		Create a new version-management data access facility.
		/// </summary>
		/// <param name="entityContext">
		///		The version management entity context.
		/// </param>
		public VersionManagementData(VersionManagementEntities entityContext)
		{
			if (entityContext == null)
				throw new ArgumentNullException(nameof(entityContext));

			_entityContext = entityContext;
		}

		/// <summary>
		///		Dispose of resources being used by the data-access API.
		/// </summary>
		public void Dispose() => _entityContext.Dispose();

		/// <summary>
		///		Get the <see cref="ReleaseVersionData"/> (if it exists) for the specified combination of product, release, and commit Id.
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
		///		The <see cref="ReleaseVersionData"/>, or <c>null</c> if no matching <see cref="ReleaseVersionData"/> was found.
		/// </returns>
		public ReleaseVersionData GetReleaseVersion(string productName, string releaseName, string commitId)
		{
			if (String.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'productName'.", nameof(productName));

			if (String.IsNullOrWhiteSpace(releaseName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'releaseName'.", nameof(releaseName));

			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			ProductData matchingProduct = _entityContext.Products.FirstOrDefault(product =>
				product.Name == productName
			);
			if (matchingProduct == null)
				throw new VersionManagementException("Product not found: '{0}'.", productName);

			ReleaseData matchingRelease = _entityContext.Releases.FirstOrDefault(release =>
				release.ProductId == matchingProduct.Id
				&&
				release.Name == releaseName
			);
			if (matchingRelease == null)
				throw new VersionManagementException("Release not found: '{0}'.", releaseName);

			ReleaseVersionData matchingVersion = _entityContext.ReleaseVersions.FirstOrDefault(version =>
				version.ReleaseId == matchingRelease.Id
				&&
				version.CommitId == commitId
			);

			return matchingVersion;
		}

		/// <summary>
		///		Get or create the <see cref="ReleaseVersionData"/> for the specified combination of product, release, and commit Id.
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
		///		The <see cref="ReleaseVersionData"/>.
		/// </returns>
		public ReleaseVersionData GetOrCreateReleaseVersion(string productName, string releaseName, string commitId)
		{
			if (String.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'productName'.", nameof(productName));

			if (String.IsNullOrWhiteSpace(releaseName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'releaseName'.", nameof(releaseName));

			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			ProductData matchingProduct = _entityContext.Products.FirstOrDefault(product =>
				product.Name == productName
			);
			if (matchingProduct == null)
				throw new VersionManagementException("Product not found: '{0}'.", productName);

			ReleaseData matchingRelease =
				_entityContext.Releases.Include(release => release.VersionRange)
					.FirstOrDefault(release =>
						release.ProductId == matchingProduct.Id
						&&
						release.Name == releaseName
					);
			if (matchingRelease == null)
				throw new VersionManagementException("Release not found: '{0}'.", releaseName);

			ReleaseVersionData matchingVersion = _entityContext.ReleaseVersions.FirstOrDefault(version =>
				version.ReleaseId == matchingRelease.Id
				&&
				version.CommitId == commitId
			);
			if (matchingVersion != null)
				return matchingVersion;

			ReleaseVersionData newVersion = matchingRelease.AllocateReleaseVersion(commitId);
			_entityContext.ReleaseVersions.Add(newVersion);

			_entityContext.SaveChanges();

			return newVersion;
		}

		/// <summary>
		///		Get the <see cref="ReleaseVersionData"/>(s) for the specified combination of product and commit Id.
		/// </summary>
		/// <param name="productName">
		///		The product name.
		/// </param>
		/// <param name="commitId">
		///		The commit Id.
		/// </param>
		/// <returns>
		///		A read-only list of matching <see cref="ReleaseVersionData"/>s.
		/// </returns>
		public IReadOnlyList<ReleaseVersionData> GetReleaseVersionsFromCommitId(string productName, string commitId)
		{
			if (String.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'productName'.", nameof(productName));

			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			return
				_entityContext.ReleaseVersions.Where(buildVersion =>
					buildVersion.Release.Product.Name == productName
					&&
					buildVersion.CommitId == commitId
				)
				.ToArray();
		}

		/// <summary>
		///		Get the <see cref="ReleaseVersionData"/>(s) for the specified combination of product and semantic version.
		/// </summary>
		/// <param name="productName">
		///		The product name.
		/// </param>
		/// <param name="semanticVersion">
		///		The semantic version.
		/// </param>
		/// <returns>
		///		A read-only list of matching <see cref="ReleaseVersionData"/>s.
		/// </returns>
		public IReadOnlyList<ReleaseVersionData> GetReleaseVersionsFromSemanticVersion(string productName, string semanticVersion)
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
				_entityContext.ReleaseVersions.Where(buildVersion =>
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
	}
}