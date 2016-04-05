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
		ReleaseVersionData GetReleaseVersion(string productName, string releaseName, string commitId);

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
		ReleaseVersionData GetOrCreateReleaseVersion(string productName, string releaseName, string commitId);

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
		IReadOnlyList<ReleaseVersionData> GetReleaseVersionsFromCommitId(string productName, string commitId);

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
		IReadOnlyList<ReleaseVersionData> GetReleaseVersionsFromSemanticVersion(string productName, string semanticVersion);
    }
}
