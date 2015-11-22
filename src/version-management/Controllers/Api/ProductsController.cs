using Microsoft.AspNet.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DD.Cloud.VersionManagement.Controllers.Api
{
	using DataAccess;
    using DataAccess.Models;

	/// <summary>
	///		Controller for the version-management Products API.
	/// </summary>
    [Route("api/v1/products")]
    public class ProductsController
		: ApiController
	{
		/// <summary>
		///		The version-management entity context.
		/// </summary>
		readonly VersionManagementEntities _entities;
		
		/// <summary>
		///		Create a new <see cref="ProductsController"/>.
		/// </summary>
		/// <param name="entities">
		///		The version-management entity context.
		/// </param>
		public ProductsController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));
			
			_entities = entities;
		}
		
		/// <summary>
		///		Get information about all products in the version-management system.
		/// </summary>
		/// <returns>
		///		Array of products.
		/// </returns>
		[HttpGet("")]
		public IActionResult GetAllProducts()
		{
			Product[] products = _entities.Products.ToArray();
			
			return Ok(products);
		}

		/// <summary>
		///		Create a new product.
		/// </summary>
		/// <param name="name">
		///		The product name.
		/// </param>
		/// <returns>
		///		The product, or
		/// </returns>
	    [HttpPost("")]
	    public async Task<IActionResult> CreateProduct([Required] string name = null)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Product product = _entities.Products.FirstOrDefault(
				existingProduct => existingProduct.Name == name
			);
			if (product != null)
			{
				Context.Response.Headers.Add("Reason",
					$"Product named '{name}' already exists."
				);

				return Conflict();
			}

			product = new Product
			{
				Name = name
			};
			_entities.Products.Add(product);
			await _entities.SaveChangesAsync();

			return Ok(product);
		}
	}
}
