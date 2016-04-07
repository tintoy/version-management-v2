using Microsoft.AspNet.Mvc;
using System.Linq;

namespace DD.Cloud.VersionManagement.Controllers
{
	using System;
	using DataAccess;
	using DataAccess.Models;
	using Microsoft.AspNet.Mvc.Rendering;
	using Microsoft.Data.Entity;
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
		///		Display all version ranges.
		/// </summary>
		/// <returns>
		///		An action result that renders the version range index view.
		/// </returns>
		public IActionResult Index()
		{
			VersionRangeModel[] versionRanges =
				_entities.VersionRanges
					.OrderBy(versionRange => versionRange.Name)
					.AsEnumerable()
					.Select(versionRange => VersionRangeModel.FromData(versionRange))
					.ToArray();

			return View(versionRanges);
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

		/// <summary>
		///		Display the version range edit view.
		/// </summary>
		/// <param name="versionRangeId">
		///		The Id of the version range to edit.
		/// </param>
		/// <returns>
		///		An action result that renders the version range creation view.
		/// </returns>
		[HttpGet("{versionRangeId:int}/edit")]
		public IActionResult Edit(int versionRangeId)
		{
			VersionRangeData versionRangeData = _entities.VersionRanges.FirstOrDefault(
				versionRange => versionRange.Id == versionRangeId
			);
			if (versionRangeData == null)
				return HttpNotFound($"No version range was found with Id {versionRangeId}");

			ViewBag.VersionComponents = GetVersionComponentSelectList(versionRangeData.IncrementBy);

			return View(
				VersionRangeModel.FromData(versionRangeData)
			);
		}

		/// <summary>
		///		Handle input from the version range edit view.
		/// </summary>
		/// <param name="versionRangeId">
		///		The Id of the version range being edited.
		/// </param>
		/// <param name="model">
		///		A <see cref="VersionRangeModel"/> representing the version range being edited.
		/// </param>
		/// <returns>
		///		An action result that renders the version range creation view.
		/// </returns>
		[HttpPost("{versionRangeId:int}/edit")]
		public IActionResult Edit(int versionRangeId, VersionRangeModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			ViewBag.VersionComponents = GetVersionComponentSelectList(model.IncrementBy);

			if (!ModelState.IsValid)
				return View(model);

			if (model.StartVersion >= model.EndVersion)
			{
				ModelState.AddModelError("StartVersion",
					"Start version must be less than end version."
				);
			}

			if (model.NextVersion < model.StartVersion)
			{
				ModelState.AddModelError("NextVersion",
					"Next version must be greater than or equal to start version."
				);
			}

			if (model.NextVersion > model.EndVersion)
			{
				ModelState.AddModelError("NextVersion",
					"Next version must be less than or equal to end version."
				);
			}

			if (!ModelState.IsValid)
				return View(model);

			VersionRangeData versionRangeData = _entities.VersionRanges.FirstOrDefault(
				versionRange => versionRange.Id == model.Id
			);
			if (versionRangeData == null)
				return HttpNotFound($"No version range was found with Id {model.Id}");

			model.ToData(versionRangeData);
			_entities.SaveChanges();

			return RedirectToAction("Index");
		}

		/// <summary>
		///		Create a <see cref="SelectList"/> containing all possible <see cref="VersionComponent"/>s.
		/// </summary>
		/// <param name="selectedVersionComponent">
		///		The currently-selected version component (if any).
		/// </param>
		/// <returns>
		///		The new <see cref="SelectList"/>.
		/// </returns>
		static SelectList GetVersionComponentSelectList(VersionComponent? selectedVersionComponent = null)
		{
			var versionComponentOptions =
				Enum.GetValues(typeof(VersionComponent))
					.Cast<VersionComponent>()
					.Where(versionComponent => versionComponent != VersionComponent.Unknown)
					.Select(versionComponent => new
					{
						Name = versionComponent.ToString(),
						Value = versionComponent
					})
					.ToArray();

			return new SelectList(versionComponentOptions,
				dataValueField: "Value",
				dataTextField: "Name",
				selectedValue: selectedVersionComponent
			);
		}
	}
}
