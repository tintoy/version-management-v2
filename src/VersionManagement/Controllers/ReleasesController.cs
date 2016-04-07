using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System.Linq;

namespace DD.Cloud.VersionManagement.Controllers
{
	using DataAccess;
	using DataAccess.Models;
	using Microsoft.AspNet.Mvc.Rendering;
	using Microsoft.Extensions.Logging;
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
		readonly VersionManagementEntities		_entities;

		/// <summary>
		///		Create a new <see cref="ReleasesController"/>.
		/// </summary>
		/// <param name="log">
		///		The logging facility for the releases controller.
		/// </param>
		/// <param name="entities">
		///		The version-management entity context.
		/// </param>
		public ReleasesController(ILogger<ReleasesController> log, VersionManagementEntities entities)
		{
			if (log == null)
				throw new System.ArgumentNullException(nameof(log));

			if (entities == null)
				throw new System.ArgumentNullException(nameof(entities));

			Log = log;
			_entities = entities;
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
			ReleaseData[] releases =
				_entities.Releases
					.Include(release => release.Product)
					.Include(release => release.VersionRange)
					.ToArray();

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
			ReleaseData[] releasesByProductId =
				_entities.Releases
					.Include(release => release.Product)
					.Include(release => release.VersionRange)
					.Where(release => release.ProductId == productId)
					.ToArray();

			return View("Index", releasesByProductId);
		}

		/// <summary>
		///		Show all releases for the specified product.
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
			ReleaseData[] releasesByVersionRangeId =
				_entities.Releases
					.Include(release => release.Product)
					.Include(release => release.VersionRange)
					.Where(release => release.VersionRangeId == versionRangeId)
					.ToArray();

			return View("Index", releasesByVersionRangeId);
		}

		[HttpGet("{releaseId:int}")]
		public IActionResult DetailById(int releaseId)
		{
			ReleaseData releaseById = 
				_entities.Releases
					.Include(release => release.Product)
					.Include(release => release.VersionRange)
					.FirstOrDefault(release => release.Id == releaseId);
			if (releaseById == null)
				return HttpNotFound($"Release {releaseId} not found.");

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
			ViewBag.Products = GetProductSelectList();
			ViewBag.VersionRanges = GetVersionRangeSelectList();

			return View(
				new ReleaseModel()
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
		public IActionResult Create(ReleaseModel model)
		{
			if (model == null)
				throw new System.ArgumentNullException(nameof(model));

			ViewBag.Products = GetProductSelectList(model.ProductId);
			ViewBag.VersionRanges = GetVersionRangeSelectList(model.VersionRangeId);

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

			ViewBag.Products = GetProductSelectList(releaseData.ProductId);
			ViewBag.VersionRanges = GetVersionRangeSelectList(releaseData.VersionRangeId);

			return View(
				ReleaseModel.FromData(releaseData)
			);
		}

		/// <summary>
		///		Handle input from the release edit view.
		/// </summary>
		/// <param name="releaseId">
		///		The Id of the release being edited.
		/// </param>
		/// <param name="model">
		///		A <see cref="ReleaseModel"/> representing the release being edited.
		/// </param>
		/// <returns>
		///		An action result that renders the release creation view.
		/// </returns>
		[HttpPost("{releaseId:int}/edit")]
		public IActionResult Edit(int releaseId, ReleaseModel model)
		{
			if (model == null)
				throw new System.ArgumentNullException(nameof(model));

			if (!ModelState.IsValid)
			{
				ViewBag.Products = GetProductSelectList(model.ProductId);
				ViewBag.VersionRanges = GetVersionRangeSelectList(model.VersionRangeId);

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

		/// <summary>
		///		Create a <see cref="SelectList"/> containing all products.
		/// </summary>
		/// <param name="selectedProductId">
		///		The Id of the currently-selected product (if any).
		/// </param>
		/// <returns>
		///		The new <see cref="SelectList"/>.
		/// </returns>
		SelectList GetProductSelectList(int? selectedProductId = null)
		{
			ProductData[] products =
				_entities.Products.AsNoTracking()
					.OrderBy(product => product.Name)
					.ToArray();

			return new SelectList(products,
				dataValueField: "Id",
				dataTextField: "Name",
				selectedValue: selectedProductId
			);
		}

		/// <summary>
		///		Create a <see cref="SelectList"/> containing all version ranges.
		/// </summary>
		/// <param name="selectedVersionRangeId">
		///		The Id of the currently-selected version range (if any).
		/// </param>
		/// <returns>
		///		The new <see cref="SelectList"/>.
		/// </returns>
		SelectList GetVersionRangeSelectList(int? selectedVersionRangeId = null)
		{
			VersionRangeData[] versionRanges =
				_entities.VersionRanges.AsNoTracking()
					.OrderBy(versionRange => versionRange.Name)
					.ToArray();

			return new SelectList(versionRanges,
				dataValueField: "Id",
				dataTextField: "Name",
				selectedValue: selectedVersionRangeId
			);
		}
	}
}
