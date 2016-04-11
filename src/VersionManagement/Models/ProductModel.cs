using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DD.Cloud.VersionManagement.Models
{
    using DataAccess.Models;

    /// <summary>
    ///		View model for a product.
    /// </summary>
    public class ProductModel
    {
		/// <summary>
		///		The product Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///		The product name.
		/// </summary>
		[Required(ErrorMessage = "Product must have a name.")]
		public string Name { get; set; }

		/// <summary>
		///		Convert the <see cref="ProductModel"/> to a new <see cref="ProductData"/>.
		/// </summary>
		/// <returns>
		///		The new <see cref="ProductData"/>.
		/// </returns>
		public ProductData ToData()
		{
			return new ProductData
			{
				Id = Id,
				Name = Name
			};
		}

		/// <summary>
		///		Update existing <see cref="ProductData"/> from the <see cref="ProductModel"/>.
		/// </summary>
		/// <param name="productData">
		///		The <see cref="ProductData"/> to update.
		/// </param>
		public void ToData(ProductData productData)
		{
			if (productData == null)
				throw new ArgumentNullException(nameof(productData));

			if (productData.Id != Id)
				throw new InvalidOperationException($"Cannot update product data for product {productData.Id} from model for product {Id} (Ids do not match).");

			productData.Name = Name;
		}

		/// <summary>
		///		Create a new <see cref="ProductModel"/> from the specified <see cref="ProductData"/>.
		/// </summary>
		/// <param name="productData">
		///		The product persistence model.
		/// </param>
		/// <returns>
		///		The new <see cref="ProductModel"/>, or <c>null</c> if the <see cref="ProductData"/> is <c>null</c>.
		/// </returns>
		public static ProductModel FromData(ProductData productData)
		{
			if (productData == null)
				return null;

			return new ProductModel
			{
				Id = productData.Id,
				Name = productData.Name
			};
		}
		
		/// <summary>
		///		Create a sequence of <see cref="ProductModel"/>s from the specified <see cref="ProductData"/>.
		/// </summary>
		/// <param name="productData">
		///		The product persistence models.
		/// </param>
		/// <returns>
		///		A sequence of <see cref="ProductModel"/>.
		/// </returns>
		public static IEnumerable<ProductModel> FromData(IEnumerable<ProductData> productData)
		{
			if (productData == null)
				throw new ArgumentNullException(nameof(productData));
				
			return productData.Select(
				data => FromData(data)
			);
		}
    }
}
