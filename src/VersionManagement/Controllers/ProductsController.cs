using Microsoft.AspNet.Mvc;
using System.Linq;

namespace DD.Cloud.VersionManagement.Controllers
{
	using DataAccess;
	using DataAccess.Models;
	using Models;

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
		/// <remarks>
		///		TODO: Switch to using <see cref="IVersionManagementData"/> (and move required functionality into it).
		/// </remarks>
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
		[HttpGet("")]
		public IActionResult Index()
		{
			ProductModel[] products =
				_entities.Products
					.Select(productData => ProductModel.FromData(productData))
					.ToArray();

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
		[HttpGet("{productId:int}", Name = "ProductById")]
		public IActionResult DetailById(int productId)
		{
			ProductData productById = _entities.Products.FirstOrDefault(
				product => product.Id == productId
			);
			if (productById == null)
				return HttpNotFound($"No product found with Id {productId}.");

			return View("Detail",
				ProductModel.FromData(productById)
			);
		}

		/// <summary>
		///		Display the product creation view.
		/// </summary>
		/// <returns>
		///		An action result that renders the product creation view.
		/// </returns>
		[HttpGet("create")]
		public IActionResult Create()
		{
			return View(
				new ProductModel()
			);
		}

		/// <summary>
		///		Handle input from the product creation view.
		/// </summary>
		/// <param name="model">
		///		The product model.
		/// </param>
		/// <returns>
		///		An action result that redirects to the product list view.
		/// </returns>
		[HttpPost("create")]
		public IActionResult Create(ProductModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			ProductData existingProductDataByName = _entities.Products.FirstOrDefault(
				existingProductData => existingProductData.Name == model.Name
			);
			if (existingProductDataByName != null)
			{
				ModelState.AddModelError("Name", $"A product already exists with name '{model.Name}'.");

				return View(model);
			}

			_entities.Products.Add(
				model.ToData()
			);
			_entities.SaveChanges();

			return RedirectToAction("Index");
		}

		/// <summary>
		///		Display the product edit view.
		/// </summary>
		/// <param name="productId">
		///		The Id of the product to edit.
		/// </param>
		/// <returns>
		///		An action result that renders the product creation view.
		/// </returns>
		[HttpGet("{productId:int}/edit")]
		public IActionResult Edit(int productId)
		{
			ProductData productData = _entities.Products.FirstOrDefault(
				product => product.Id == productId
			);
			if (productData == null)
				return HttpNotFound($"No product was found with Id {productId}");

			return View(
				ProductModel.FromData(productData)
			);
		}

		/// <summary>
		///		Handle input from the product edit view.
		/// </summary>
		/// <param name="productId">
		///		The Id of the product being edited.
		/// </param>
		/// <param name="model">
		///		A <see cref="ProductModel"/> representing the product being edited.
		/// </param>
		/// <returns>
		///		An action result that renders the product creation view.
		/// </returns>
		[HttpPost("{productId:int}/edit")]
		public IActionResult Edit(int productId, ProductModel model)
		{
			if (model == null)
				throw new System.ArgumentNullException(nameof(model));

			if (!ModelState.IsValid)
				return View(model);

			ProductData productData = _entities.Products.FirstOrDefault(
				product => product.Id == model.Id
			);
			if (productData == null)
				return HttpNotFound($"No product was found with Id {model.Id}");

			model.ToData(productData);
			_entities.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
