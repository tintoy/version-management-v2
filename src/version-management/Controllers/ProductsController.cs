using Microsoft.AspNet.Mvc;
using System.Linq;

namespace DD.Cloud.VersionManagement.Controllers
{
	using DataAccess;
	using DataAccess.Models;

	/// <summary>
	///		The products controller.
	/// </summary>
	[Route("products")]
    public class ProductsController
		: Controller
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
				throw new System.ArgumentNullException(nameof(entities));

			_entities = entities;
		}

		/// <summary>
		///		Show all products.
		/// </summary>
		/// <returns>
		///		An action result that renders the product index view.
		/// </returns>
		[Route("")]
		public IActionResult Index()
		{
			Product[] products = _entities.Products.ToArray();

            return View(products);
        }

		/// <summary>
		///		Show the detail for a specific product.
		/// </summary>
		/// <param name="productId">
		///		The product Id.
		/// </param>
		/// <returns>
		///		An action result that renders the product detail view.
		/// </returns>
		[Route("{productId:int}", Name = "ProductById")]
		public IActionResult DetailById(int productId)
		{
			Product productById = _entities.Products.FirstOrDefault(
				product => product.Id == productId
			);
			if (productById == null)
				return HttpNotFound($"No product found with Id {productId}.");

			return View("Detail", productById);
		}
    }
}
