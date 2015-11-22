using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using System.Web.Http;

namespace DD.Cloud.VersionManagement.Controllers.Api
{    
    using DataAccess;
    using DataAccess.Models;

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
				throw new ArgumentNullException("entities");
			
			_entities = entities;
		}
		
		/// <summary>
		///		Get information about all products in the version-management system.
		/// </summary>
		/// <returns>
		///		The action result (array of products).
		/// </returns>
		[HttpGet("")]
		public IActionResult GetAllProducts()
		{
			Product[] products = _entities.Products.ToArray();
			
			return Ok(products);
		}
	}
}