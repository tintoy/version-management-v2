using System.Collections.Generic;

namespace DD.Cloud.VersionManagement.DataAccess
{
	using Models;

	/// <summary>
	///		Represents the version-management data access facility.
	/// </summary>
    public interface IVersionManagementData
    {
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
		ReleaseVersion GetReleaseVersion(string productName, string releaseName, string commitId);

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
		ReleaseVersion GetOrCreateReleaseVersion(string productName, string releaseName, string commitId);

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
		IReadOnlyList<ReleaseVersion> GetReleaseVersionsFromCommitId(string productName, string commitId);

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
		IReadOnlyList<ReleaseVersion> GetReleaseVersionsFromSemanticVersion(string productName, string semanticVersion);
    }
}
