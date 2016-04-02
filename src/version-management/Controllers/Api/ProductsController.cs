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
	///		The products API controller.
	/// </summary>
	[Route("api/v2/[controller]")]
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
		///		Get a <see cref="Product"/> by name.
		/// </summary>
		/// <param name="productName">
		///		The product name.
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpGet("")]
		public IActionResult Get(string productName)
		{
			if (String.IsNullOrWhiteSpace(productName))
			{
				Product[] products = _entities.Products.ToArray();

				return Ok(products);
			}

			Product productByName = _entities.Products.FirstOrDefault(
				product => product.Name == productName
			);
			if (productByName != null)
				return Ok(productByName);

			return EntityNotFound(new
			{
				Message = $"No entity was found named '{productName}'.",
				ProductName = productName,
				ErrorCode = "EntityNotFound"
			});
		}

		/// <summary>
		///		Get a <see cref="Product"/> by Id.
		/// </summary>
		/// <param name="productId">
		///		The product Id.
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpGet("{productId:int}")]
		public IActionResult GetById([Required] int productId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Product matchingProduct = _entities.Products.FirstOrDefault(
				product => product.Id == productId
			);
			if (matchingProduct != null)
				return Ok(matchingProduct);

			return NotFound();
		}

		/// <summary>
		///		Create a new <see cref="Product"/>.
		/// </summary>
		/// <param name="name">
		///		A unique name for the new product.
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpPost("")]
		public async Task<IActionResult> Create([Required] string name = null)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Product product = _entities.Products.FirstOrDefault(
				existingProduct => existingProduct.Name == name
			);
			if (product != null)
			{
				Context.Response.Headers.Add("X-ErrorCode",
					"EntityAlreadyExists"
				);
				Context.Response.Headers.Add("X-Reason",
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
		///		Update an existing <see cref="Product"/>.
		/// </summary>
		/// <param name="id">
		///		The Id of the <see cref="Product"/> to update.
		/// </param>
		/// <param name="name">
		///		The product name.
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update([Required] int id, [Required] string name = null)
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
		///		Delete an existing <see cref="Product"/>.
		/// </summary>
		/// <param name="id">
		///		The Id of the <see cref="Product"/> to delete.
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpDelete("{id:int}")]
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

		/// <summary>
		///		Create an action result representing an entity that was not found.
		/// </summary>
		/// <typeparam name="TBody">
		///		The response body type.
		/// </typeparam>
		/// <param name="body">
		///		The response body.
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		/// <remarks>
		///		TODO: Move this to a shared base class.
		/// </remarks>
		IActionResult EntityNotFound<TBody>(TBody body)
		{
			Context.Response.Headers["X-ErrorCode"] = "EntityNotFound";
			
			// TODO: Add X-EntityType header.

			return new HttpNotFoundObjectResult(body);
		}
	}
}
