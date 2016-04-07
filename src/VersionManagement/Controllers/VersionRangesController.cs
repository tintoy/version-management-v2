using Microsoft.AspNet.Mvc;
using System.Linq;

namespace DD.Cloud.VersionManagement.Controllers
{
	using DataAccess;
	using DataAccess.Models;
	using Models;

	/// <summary>
	///		The version ranges controller.
	/// </summary>
	[Route("version-ranges")]
	public class VersionRangesController
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
		///		Create a new <see cref="VersionRangesController"/>.
		/// </summary>
		/// <param name="entities">
		///		The version-management entity context.
		/// </param>
		public VersionRangesController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new System.ArgumentNullException(nameof(entities));

			_entities = entities;
		}

		/// <summary>
		///		Show the detail for a specific  vdrsion range.
		/// </summary>
		/// <param name="versionRangeId">
		///		The version range Id.
		/// </param>
		/// <returns>
		///		An action result that renders the version range detail view.
		/// </returns>
		[HttpGet("{versionRangeId:int}")]
		public IActionResult DetailById(int versionRangeId)
		{
			VersionRangeData versionRangeById = _entities.VersionRanges.FirstOrDefault(
				versionRange => versionRange.Id == versionRangeId
			);
			if (versionRangeById == null)
				return HttpNotFound($"No product found with Id {versionRangeId}.");

			return View("Detail",
				VersionRangeModel.FromData(versionRangeById)
			);
		}
	}
}
