using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DD.Cloud.VersionManagement.Controllers
{
	using DataAccess;
	using Models;

	/// <summary>
	///		The products controller.
	/// </summary>
	[Route("products")]
	public class ProductsController
		: ControllerBase
	{
		/// <summary>
		///     The version-management data access facility.
		/// </summary>
		readonly IVersionManagementData     _data;

		/// <summary>
		///		Create a new <see cref="ProductsController"/>.
		/// </summary>
		/// <param name="data">
		///		The version-management data access facility.
		/// </param>
		/// <param name="log">
		///		The controller's log facility.
		/// </param>
		public ProductsController(IVersionManagementData data, ILogger<ProductsController> log)
			: base(log)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));
				
			_data = data;
		}

		/// <summary>
		///		Display all products.
		/// </summary>
		/// <returns>
		///		An action result that renders the product index view.
		/// </returns>
		[HttpGet("")]
		public IActionResult Index()
		{
			IReadOnlyList<ProductModel> products = _data.GetProducts();
			
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
			ProductModel productById = _data.GetProductById(productId);
			if (productById == null)
				return HttpNotFound($"No product found with Id {productId}.");

			return View("Detail", productById);
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

			ProductModel existingProductByName = _data.GetProductByName(model.Name);
			if (existingProductByName != null)
			{
				ModelState.AddModelError(nameof(model.Name),
					$"A product already exists with name '{model.Name}'."
				);

				return View(model);
			}

			ProductModel newProduct = _data.CreateProduct(model.Name);
			
			Log.LogInformation("Created new product '{ProductName}' (Id = {ProductId}).",
				newProduct.Name,
				newProduct.Id
			);

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
			ProductModel product = _data.GetProductById(productId);
			if (product == null)
				return HttpNotFound($"No product was found with Id {productId}");

			return View(product);
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
				throw new ArgumentNullException(nameof(model));

			if (!ModelState.IsValid)
				return View(model);

			model.Id = productId;

			ProductModel updatedProduct = _data.UpdateProduct(model);
			if (updatedProduct == null)
				return HttpNotFound($"No product was found with Id {model.Id}");

			return RedirectToAction("Index");
		}
	}
}
