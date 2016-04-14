using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DD.Cloud.VersionManagement.DataAccess
{
	using DD.Cloud.VersionManagement.Models;
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
		
		#region Products
		
		/// <summary>
		///     Get all known products.
		/// </summary>
		/// <returns>
		///     A read-only list of <see cref="ProductModel"/>s represdenting the products (sorted by name).
		/// </returns>
		public IReadOnlyList<ProductModel> GetProducts()
		{
			return
				ProductModel.FromData(
					_entityContext.Products
						.OrderBy(product => product.Name)
				)
				.ToArray();
		}
		
		/// <summary>
		///     Get a specific product by Id.
		/// </summary>
		/// <returns>
		///     A <see cref="ProductModel"/> representing the product, or <c>null</c> if no product was found with the specified Id.
		/// </returns>
		public ProductModel GetProductById(int productId)
		{
			ProductData productById = _entityContext.Products.FirstOrDefault(
				product => product.Id == productId
			);
			if (productById == null)
				return null;
				
			return ProductModel.FromData(productById);
		}

		/// <summary>
		///     Get a specific product by name.
		/// </summary>
		/// <returns>
		///     A <see cref="ProductModel"/> representing the product, or <c>null</c> if no product was found with the specified name.
		/// </returns>
		public ProductModel GetProductByName(string productName)
		{
			if (productName == null)
				throw new ArgumentNullException(nameof(productName));
			
			ProductData productByName = _entityContext.Products.FirstOrDefault(
				product => product.Name == productName
			);
			if (productByName == null)
				return null;
				
			return ProductModel.FromData(productByName);
		}
		
		/// <summary>
		///     Create a new product.
		/// </summary>
		/// <param name="productName">
		///     The name for the new product.
		/// </param>
		/// <returns>
		///     A <see cref="ProductModel"/> representing the new product.
		/// </returns>
		public ProductModel CreateProduct(ProductModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model)); 
				
			ProductData productData = model.ToData();
			
			_entityContext.Products.Add(productData);
			_entityContext.SaveChanges();
			
			return ProductModel.FromData(productData);
		}
		
		/// <summary>
		/// 	Update the specified product.
		/// </summary>
		/// <param name="product">
		///		A <see cref="ProductModel"/> representing the product to update.
		/// </param>
		/// <returns>
		///		A <see cref="ProductModel"/> representing the updated product, or <c>null</c> if the product was not found by Id.
		/// </returns>
		public ProductModel UpdateProduct(ProductModel product)
		{
			if (product == null)
				throw new ArgumentNullException(nameof(product));
				
			if (String.IsNullOrWhiteSpace(product.Name))
				throw new ArgumentException("Product name cannot be null, empty, or entirely composed of whitespace.", nameof(product));
				
			ProductData productById = _entityContext.Products.FirstOrDefault(
				productData => productData.Id == product.Id
			);
			if (productById == null)
				return null;
				
			product.ToData(productById);
			_entityContext.SaveChanges();
			
			return ProductModel.FromData(productById);
		}
		
		#endregion // Products
		
		#region Releases
		
		/// <summary>
		/// 	Get all releases.
		/// </summary>
		/// <returns>
		///		A read-only list of releases (sorted by release name, then product name).
		/// </returns>
		public IReadOnlyList<ReleaseDisplayModel> GetReleases()
		{
			IQueryable<ReleaseData> releases =
				_entityContext.Releases
					.Include(release => release.Product)
					.Include(release => release.VersionRange)
					.OrderBy(release => release.Product.Name)
					.ThenBy(release => release.Name);
					
			return
				ReleaseDisplayModel.FromData(releases)
					.ToArray();
		}
		
		/// <summary>
		/// 	Get all releases associated with the specified product.
		/// </summary>
		/// <param name="productId">
		///		The product Id.
		/// </param>
		/// <returns>
		///		A read-only list of releases (sorted by release name, then product name).
		/// </returns>
		public IReadOnlyList<ReleaseDisplayModel> GetReleasesByProduct(int productId)
		{
			IQueryable<ReleaseData> releases =
				_entityContext.Releases
					.Include(release => release.Product)
					.Include(release => release.VersionRange)
					.Where(release => release.ProductId == productId)
					.OrderBy(release => release.Product.Name)
					.ThenBy(release => release.Name);
			return
				ReleaseDisplayModel.FromData(releases)
					.ToArray();
		}
		
		/// <summary>
		/// 	Get all releases associated with the specified version range.
		/// </summary>
		/// <param name="versionRangeId">
		///		The version range Id.
		/// </param>
		/// <returns>
		///		A read-only list of releases (sorted by release name, then product name).
		/// </returns>
		public IReadOnlyList<ReleaseDisplayModel> GetReleasesByVersionRange(int versionRangeId)
		{
			IQueryable<ReleaseData> releases =
				_entityContext.Releases
					.Include(release => release.Product)
					.Include(release => release.VersionRange)
					.Where(release => release.VersionRangeId == versionRangeId)
					.OrderBy(release => release.Product.Name)
					.ThenBy(release => release.Name);
				
			return
				ReleaseDisplayModel.FromData(releases)
					.ToArray();
		}
		
		/// <summary>
		/// 	Get a specific release by Id.
		/// </summary>
		/// <param name="releaseId">
		///		The release Id.
		/// </param>
		/// <returns>
		///		A <see cref="ReleaseDisplayModel"/> representing the release, or <c>null<c> if no release was found with the specified Id.
		/// </returns>
		public ReleaseDisplayModel GetReleaseById(int releaseId)
		{
			ReleaseData releaseById =
				_entityContext.Releases
					.Include(release => release.Product)
					.Include(release => release.VersionRange)
					.FirstOrDefault(release => release.Id == releaseId);
					
			return ReleaseDisplayModel.FromData(releaseById);
		}
		
		/// <summary>
		/// 	Create a new release.
		/// </summary>
		/// <param name="model">
		///		A <see cref="ReleaseEditModel"/> representing the release to create.
		/// </param>
		/// <returns>
		///		A <see cref="ReleaseDisplayModel"/> representing the new release.
		/// </returns>
		public ReleaseDisplayModel CreateRelease(ReleaseEditModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
				
			if (!model.ProductId.HasValue)
				throw new ArgumentException("Release is missing associated product Id.", nameof(model));
				
			ProductModel product = GetProductById(model.ProductId.Value);
			if (product == null)
				throw EntityNotFoundException.Product(model.ProductId.Value);
				
			ReleaseData existingReleaseByName = _entityContext.Releases.FirstOrDefault(
				release => release.Name == model.Name && release.ProductId == model.ProductId
			);
			if (existingReleaseByName != null)
				throw EntityAlreadyExistsException.ReleaseWithName(model.Name, product.Name);

			ReleaseData newRelease = model.ToData();
			
			_entityContext.Releases.Add(
				model.ToData()
			);
			_entityContext.SaveChanges();
			
			return GetReleaseById(newRelease.Id);
		}
		
		#endregion // Releases

		#region ReleaseVersions

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
		
		#endregion // ReleaseVersions
	}
}