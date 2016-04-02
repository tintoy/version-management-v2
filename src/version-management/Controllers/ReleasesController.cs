using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System.Linq;

namespace DD.Cloud.VersionManagement.Controllers
{
	using DataAccess;
	using DataAccess.Models;

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
		readonly VersionManagementEntities _entities;

		/// <summary>
		///		Create a new <see cref="ReleasesController"/>.
		/// </summary>
		/// <param name="entities">
		///		The version-management entity context.
		/// </param>
		public ReleasesController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new System.ArgumentNullException(nameof(entities));

			_entities = entities;
		}

		/// <summary>
		///		Show all releases.
		/// </summary>
		/// <returns>
		///		An action result that renders the release index view.
		/// </returns>
		[Route("")]
		public IActionResult Index()
		{
			Release[] releases =
				_entities.Releases.Include(
					release => release.Product
				)
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
		[Route("product/{productId:int}")]
		public IActionResult IndexByProduct(int productId)
		{
			Release[] releasesByProductId =
				_entities.Releases.Include(
					release => release.Product
				)
				.Where(
					release => release.ProductId == productId
				)
				.ToArray();

			return View("Index", releasesByProductId);
		}

		[Route("{releaseId:int}")]
		public IActionResult DetailById(int releaseId)
		{
			Release releaseById =
				_entities.Releases.Include(
					release => release.Product
				)
				.FirstOrDefault(
					release => release.Id == releaseId
				);
			if (releaseById == null)
				return HttpNotFound($"Release {releaseId} not found.");

			return View("Detail", releaseById);
		}
	}
}
