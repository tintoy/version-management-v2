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

		public IActionResult Index()
		{
			Product[] products = _entities.Products.ToArray();

            return View(products);
        }
    }
}
