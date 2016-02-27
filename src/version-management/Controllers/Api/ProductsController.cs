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

	[Route("api/v2/products")]
	public class ProductsController
		: ApiController
	{
		readonly VersionManagementEntities _entities;

		public ProductsController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));

			_entities = entities;
		}

		[HttpGet("")]
		public IActionResult GetAllProducts()
		{
			Product[] products = _entities.Products.ToArray();

			return Ok(products);
		}

		[HttpGet("{id:int?}")]
		public IActionResult GetProductById([Required] int id)
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

		[HttpGet("")]
		public IActionResult GetProductByName([Required] string name)
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

		[HttpPut("{id:int?}")]
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
