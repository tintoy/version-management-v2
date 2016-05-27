using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DD.Cloud.VersionManagement.Controllers.Api
{
	using DataAccess;
	using DataAccess.Models;

	/// <summary>
	/// 	The releases API controller.
	/// </summary>
	[Route("api/v2/[controller]")]
	public class ReleasesController
		: ControllerBase
	{
		/// <summary>
		/// 	The version-management entity context.
		/// </summary>
		readonly VersionManagementEntities _entities;

		/// <summary>
		/// 	Create a new <see cref="ReleasesController"/>.
		/// </summary>
		/// <param name="entities">
		///		The version-management entity context.
		/// </param>
		public ReleasesController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));

			_entities = entities;
		}

		/// <summary>
		/// 	Get information about one or more releases.
		/// </summary>
		/// <param name="productName">
		///		The name of the product that owns the release. 
		/// </param>
		/// <param name="releaseName">
		///		The optional name of a specific release (otherwise, all releases for the specified product are retrieved). 
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpGet("")]
		public IActionResult Get([Required] string productName = null, string releaseName = null)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			ReleaseData[] releases;
			if (!String.IsNullOrWhiteSpace(releaseName))
			{
				releases =
					_entities.Releases.Where(release =>
						release.Product.Name == productName
						&&
						release.Name == releaseName
					)
					.ToArray();
			}
			else
			{
				releases =
					_entities.Releases.Where(release =>
						release.Product.Name == productName
					)
					.ToArray();
			}

			return Ok(releases);
		}

		/// <summary>
		/// 	Get information a release using its Id.
		/// </summary>
		/// <param name="releaseId">
		///		The Id of the target release. 
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpGet("{id:int}")]
		public IActionResult GetReleaseById([Required] int id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			ReleaseData matchingRelease = _entities.Releases.FirstOrDefault(
				Release => Release.Id == id
			);
			if (matchingRelease != null)
				return Ok(matchingRelease);

			return NotFound();
		}
		
		/// <summary>
		/// 	Create a new release.
		/// </summary>
		/// <param name="productId">
		///		The Id of the product that will own the release. 
		/// </param>
		/// <param name="releaseName">
		///		A unique (per product) name for the new release. 
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpPost("")]
		public async Task<IActionResult> Create([Required] int productId, [Required] string releaseName = null)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			ReleaseData Release = _entities.Releases.FirstOrDefault(
				existingRelease => existingRelease.Name == releaseName && existingRelease.ProductId == productId
			);
			if (Release != null)
			{
				Context.Response.Headers.Add("X-Reason",
					$"Release named '{releaseName}' already exists for ProductId '{productId}'."
				);

				return Conflict();
			}

			Release = new ReleaseData
			{
				Name = releaseName,
				ProductId = productId
			};
			_entities.Releases.Add(Release);
			await _entities.SaveChangesAsync();

			return Ok(Release);
		}

		/// <summary>
		/// 	Update an existing release.
		/// </summary>
		/// <param name="productId">
		///		The Id of the release to update. 
		/// </param>
		/// <param name="releaseName">
		///		A unique (per product) name for the release. 
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpPut("{id:int?}")]
		public async Task<IActionResult> Update([Required] int releaseId, [Required] string name = null)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			ReleaseData existingRelease = _entities.Releases.FirstOrDefault(
				matchingRelease => matchingRelease.Id == releaseId
			);
			if (existingRelease == null)
				return NotFound();

			existingRelease.Name = name;
			await _entities.SaveChangesAsync();

			return Ok(existingRelease);
		}

		/// <summary>
		/// 	Delete an existing release.
		/// </summary>
		/// <param name="releaseId">
		///		The Id of the release to delete. 
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpDelete("{id:int?}")]
		public async Task<IActionResult> Delete([Required, FromUri] int releaseId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			ReleaseData matchingRelease = _entities.Releases.FirstOrDefault(
				Release => Release.Id == releaseId
			);
			if (matchingRelease == null)
				return NotFound();

			_entities.Releases.Remove(matchingRelease);
			await _entities.SaveChangesAsync();

			return Ok();
		}
	}
}
