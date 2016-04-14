using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DD.Cloud.VersionManagement.Controllers
{
	using DataAccess;
	using DataAccess.Models;
	using Models;

	/// <summary>
	///		The releases controller.
	/// </summary>
	[Route("releases")]
	public class ReleasesController
		: Controller
	{
		/// <summary>
		///		The version-management entity context.
		/// </summary>
		readonly VersionManagementEntities	_entities;
		
		/// <summary>
		///     The version-management data access facility.
		/// </summary>
		readonly IVersionManagementData		_data;

		/// <summary>
		///		Create a new <see cref="ReleasesController"/>.
		/// </summary>
		/// <param name="data">
		///		The version-management data access facility.
		/// </param>
		/// <param name="entities">
		///		The version-management entity context.
		/// </param>
		/// <param name="log">
		///		The logging facility for the releases controller.
		/// </param>
		public ReleasesController(IVersionManagementData data, VersionManagementEntities entities, ILogger<ReleasesController> log)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));
			
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));
			
			if (log == null)
				throw new ArgumentNullException(nameof(log));
				
			Log = log;
			_entities = entities;
			_data = data;
		}

		/// <summary>
		///		The logging facility for the releases controller.
		/// </summary>
		ILogger<ReleasesController> Log { get; }

		/// <summary>
		///		Show all releases.
		/// </summary>
		/// <returns>
		///		An action result that renders the release index view.
		/// </returns>
		[HttpGet("")]
		public IActionResult Index()
		{
			IReadOnlyList<ReleaseDisplayModel> releases = _data.GetReleases();
			
			return View(releases);
		}

		/// <summary>
		///		Show all releases for the specified product.
		/// </summary>
		/// <param name="productId">
		///		The product Id.
		/// </param>
		/// <returns>
		///		An action result that renders the release index view.
		/// </returns>
		[HttpGet("product/{productId:int}")]
		public IActionResult IndexByProduct(int productId)
		{
			IReadOnlyList<ReleaseDisplayModel> releases = _data.GetReleasesByProduct(productId);
			
			return View("Index", releases);
		}

		/// <summary>
		///		Show all releases for the specified version range.
		/// </summary>
		/// <param name="versionRangeId">
		///		The product Id.
		/// </param>
		/// <returns>
		///		An action result that renders the release index view.
		/// </returns>
		[HttpGet("version-range/{versionRangeId:int}")]
		public IActionResult IndexByVersionRange(int versionRangeId)
		{
			IReadOnlyList<ReleaseDisplayModel> releases = _data.GetReleasesByVersionRange(versionRangeId);

			return View("Index", releases);
		}

		/// <summary>
		///		Render the detail view for a specific release.
		/// </summary>
		/// <param name="releaseId">
		///		The release Id.
		/// </param>
		[HttpGet("{releaseId:int}")]
		public IActionResult DetailById(int releaseId)
		{
            ReleaseDisplayModel releaseById = _data.GetReleaseById(releaseId);
			
			return View("Detail", releaseById);
		}

		/// <summary>
		///		Render the release-creation view.
		/// </summary>
		/// <returns>
		///		An action result that renders the view.
		/// </returns>
		[HttpGet("create")]
		public IActionResult Create()
		{
			ViewBag.Products = SelectLists.Products(_entities);
			ViewBag.VersionRanges = SelectLists.VersionRanges(_entities);

			return View(
				new ReleaseEditModel()
			);
		}

		/// <summary>
		///		Handle input from the release creation view.
		/// </summary>
		/// <param name="model">
		///		The release model.
		/// </param>
		/// <returns>
		///		An action result that redirects to the release list view.
		/// </returns>
		[HttpPost("create")]
		public IActionResult Create(ReleaseEditModel model)
		{
			if (model == null)
				throw new System.ArgumentNullException(nameof(model));

			ViewBag.Products = SelectLists.Products(_entities, model.ProductId);
			ViewBag.VersionRanges = SelectLists.VersionRanges(_entities, model.VersionRangeId);

			if (!ModelState.IsValid)
				return View(model);

			ProductData existingProduct = _entities.Products.FirstOrDefault(
				product => product.Id == model.ProductId
			);
			if (existingProduct == null)
			{
				ModelState.AddModelError("ProductId",
					$"No product was found with Id '{model.ProductId}'."
				);

				return View(model);
			}

			ReleaseData existingReleaseByName = _entities.Releases.FirstOrDefault(
				release => release.Name == model.Name && release.ProductId == model.ProductId
			);
			if (existingReleaseByName != null)
			{
				ModelState.AddModelError("Name",
					$"There is already a release named '{model.Name}' for product '{existingProduct.Name}'."
				);

				return View(model);
			}

			Log.LogInformation("Create release '{ReleaseName}' for product {ProductId} ('{ProductName}').",
				model.Name,
				model.ProductId,
				existingProduct.Name
			);

			_entities.Releases.Add(
				model.ToData()
			);
			_entities.SaveChanges();

			return RedirectToAction("Index");
		}

		/// <summary>
		///		Display the release edit view.
		/// </summary>
		/// <param name="releaseId">
		///		The Id of the release to edit.
		/// </param>
		/// <returns>
		///		An action result that renders the release creation view.
		/// </returns>
		[HttpGet("{releaseId:int}/edit")]
		public IActionResult Edit(int releaseId)
		{
			ReleaseData releaseData = _entities.Releases.FirstOrDefault(
				release => release.Id == releaseId
			);
			if (releaseData == null)
				return HttpNotFound($"No release was found with Id {releaseId}");

			ViewBag.Products = SelectLists.Products(_entities, releaseData.ProductId);
			ViewBag.VersionRanges = SelectLists.VersionRanges(_entities, releaseData.VersionRangeId);

			return View(
				ReleaseEditModel.FromData(releaseData)
			);
		}

		/// <summary>
		///		Handle input from the release edit view.
		/// </summary>
		/// <param name="releaseId">
		///		The Id of the release being edited.
		/// </param>
		/// <param name="model">
		///		A <see cref="ReleaseEditModel"/> representing the release being edited.
		/// </param>
		/// <returns>
		///		An action result that renders the release creation view.
		/// </returns>
		[HttpPost("{releaseId:int}/edit")]
		public IActionResult Edit(int releaseId, ReleaseEditModel model)
		{
			if (model == null)
				throw new System.ArgumentNullException(nameof(model));

			if (!ModelState.IsValid)
			{
				ViewBag.Products = SelectLists.Products(_entities, model.ProductId);
				ViewBag.VersionRanges = SelectLists.VersionRanges(_entities, model.VersionRangeId);

				return View(model);
			}

			ReleaseData releaseData = _entities.Releases.FirstOrDefault(
				release => release.Id == model.Id
			);
			if (releaseData == null)
				return HttpNotFound($"No release was found with Id {model.Id}");

			model.ToData(releaseData);
			_entities.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
