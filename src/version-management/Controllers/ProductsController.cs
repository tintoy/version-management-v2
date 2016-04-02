using Microsoft.AspNet.Mvc;
using System.Linq;

namespace DD.Cloud.VersionManagement.Controllers
{
	using DataAccess;
	using DataAccess.Models;

	[Route("products")]
    public class ProductsController
		: Controller
	{
		readonly VersionManagementEntities _entities;

		public ProductsController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new System.ArgumentNullException(nameof(entities));

			_entities = entities;
		}

		[Route("")]
		public IActionResult Index()
		{
			Product[] products = _entities.Products.ToArray();

            return View(products);
        }

		[Route("{productId:int}", Name = "ProductById")]
		public IActionResult ById(int productId)
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
