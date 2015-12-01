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
		///		Get a product by Id.
		/// </summary>
		/// <param name="id">
		///		The product Id.
		/// </param>
		[HttpGet("{id:int?}")]
		public IActionResult GetProductById([Required, FromUri] int id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
				
			Product matchingProduct = _entities.Products.FirstOrDefault(
				product => product.Id == id
			);
			if (matchingProduct != null)
				return Ok(matchingProduct);
				
			return NotFound();
		}
		
		/// <summary>
		///		Get a product by name.
		/// </summary>
		/// <param name="name">
		///		The product name.
		/// </param>
		[HttpGet("")]
		public IActionResult GetProductByName([Required, FromQuery] string name)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
				
			Product matchingProduct = _entities.Products.FirstOrDefault(
				product => product.Name == name
			);
			if (matchingProduct != null)
				return Ok(matchingProduct);
				
			return NotFound();
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
	    public async Task<IActionResult> Create([Required, FromQuery] string name)
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
		
		/// <summary>
		///		Update an existing product.
		/// </summary>
		/// <param name="id">
		///		The product Id.
		/// </param>
		/// <param name="name">
		///		The product name.
		/// </param>
		/// <returns>
		///		The product, or
		/// </returns>
	    [HttpPut("{id:int?}")]
	    public async Task<IActionResult> Update([Required, FromUri] int id, [Required, FromQuery] string name)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Product existingProduct = _entities.Products.FirstOrDefault(
				matchingProduct => matchingProduct.Id == id
			);
			if (existingProduct == null)
				return NotFound();

			existingProduct.Name = name;
			await _entities.SaveChangesAsync();

			return Ok(existingProduct);
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
	    [HttpDelete("{id:int?}")]
	    public async Task<IActionResult> Delete([Required, FromUri] int id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Product matchingProduct = _entities.Products.FirstOrDefault(
				product => product.Id == id
			);
			if (matchingProduct == null)
				return NotFound();

			_entities.Products.Remove(matchingProduct);
			await _entities.SaveChangesAsync();

			return Ok();
		}
	}
}
