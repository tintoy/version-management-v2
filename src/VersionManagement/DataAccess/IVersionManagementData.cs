﻿using System.Collections.Generic;

namespace DD.Cloud.VersionManagement.DataAccess
{
	using DD.Cloud.VersionManagement.Models;
	using Models;

	/// <summary>
	///		Represents the version-management data access facility.
	/// </summary>
	public interface IVersionManagementData
	{
		#region Products
		
		/// <summary>
		///     Get all known products.
		/// </summary>
		/// <returns>
		///     A read-only list of <see cref="ProductModel"/>s represdenting the products (sorted by name).
		/// </returns>
		IReadOnlyList<ProductModel> GetProducts();
		
		/// <summary>
		///     Get a specific product by Id.
		/// </summary>
		/// <returns>
		///     A <see cref="ProductModel"/> representing the product, or <c>null</c> if no product was found with the specified Id.
		/// </returns>
		ProductModel GetProductById(int productId);
		
		/// <summary>
		///     Get a specific product by name.
		/// </summary>
		/// <returns>
		///     A <see cref="ProductModel"/> representing the product, or <c>null</c> if no product was found with the specified name.
		/// </returns>
		ProductModel GetProductByName(string productName);
		
		/// <summary>
		///     Create a new product.
		/// </summary>
		/// <param name="productName">
		///     The name for the new product.
		/// </param>
		/// <returns>
		///     A <see cref="ProductModel"/> representing the new product.
		/// </returns>
		ProductModel CreateProduct(ProductModel model);
		
		/// <summary>
		/// 	Update the specified product.
		/// </summary>
		/// <param name="product">
		///		A <see cref="ProductModel"/> representing the product to update.
		/// </param>
		/// <returns>
		///		A <see cref="ProductModel"/> representing the updated product, or <c>null</c> if the product was not found by Id.
		/// </returns>
		ProductModel UpdateProduct(ProductModel product);
		
		#endregion // Products
		
		#region Releases
		
		/// <summary>
		/// 	Get all releases.
		/// </summary>
		/// <returns>
		///		A read-only list of releases (sorted by release name, then product name).
		/// </returns>
		IReadOnlyList<ReleaseDisplayModel> GetReleases();
		
		/// <summary>
		/// 	Get all releases associated with the specified product.
		/// </summary>
		/// <param name="productId">
		///		The product Id.
		/// </param>
		/// <returns>
		///		A read-only list of releases (sorted by release name, then product name).
		/// </returns>
		IReadOnlyList<ReleaseDisplayModel> GetReleasesByProduct(int productId);
		
		/// <summary>
		/// 	Get all releases associated with the specified version range.
		/// </summary>
		/// <param name="versionRangeId">
		///		The version range Id.
		/// </param>
		/// <returns>
		///		A read-only list of releases (sorted by release name, then product name).
		/// </returns>
		IReadOnlyList<ReleaseDisplayModel> GetReleasesByVersionRange(int versionRangeId);
		
        /// <summary>
		/// 	Get a specific release by Id.
		/// </summary>
		/// <param name="releaseId">
		///		The release Id.
		/// </param>
		/// <returns>
		///		A <see cref="ReleaseDisplayModel"/> representing the release, or <c>null<c> if no release was found with the specified Id.
		/// </returns>
		ReleaseDisplayModel GetReleaseById(int releaseId);
        
        /// <summary>
		/// 	Create a new release.
		/// </summary>
		/// <param name="model">
		///		A <see cref="ReleaseEditModel"/> representing the release to create.
		/// </param>
		/// <returns>
		///		A <see cref="ReleaseDisplayModel"/> representing the new release.
		/// </returns>
		ReleaseDisplayModel CreateRelease(ReleaseEditModel model);
        
		#endregion // Releases
		
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
