using Microsoft.AspNet.Mvc;
using System.Linq;

namespace DD.Cloud.VersionManagement.Controllers
{
	using DataAccess;
	using DataAccess.Models;
	using Microsoft.Data.Entity;

	[Route("releases")]
    public class ReleasesController
		: Controller
	{
		readonly VersionManagementEntities _entities;

		public ReleasesController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new System.ArgumentNullException(nameof(entities));

			_entities = entities;
		}

		[Route("", Name = "Index")]
		public IActionResult Index()
		{
			Release[] releases =
				_entities.Releases.Include(
					release => release.Product
				)
				.ToArray();

            return View(releases);
        }

		[Route("{releaseId:int}")]
		public IActionResult ById(int releaseId)
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

		[Route("product/{productId:int}")]
		public IActionResult ByProduct(int productId)
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
	}
}
