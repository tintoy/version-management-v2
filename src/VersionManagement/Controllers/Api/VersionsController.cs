using Microsoft.AspNet.Mvc;
using System;
using System.Web.Http;

namespace DD.Cloud.VersionManagement.Controllers.Api
{
	using DataAccess;
	using DataAccess.Models;

	/// <summary>
	/// 	The versions API controller.
	/// </summary>
	[Route("api/v2/[controller]")]
	public class VersionsController
	  : ControllerBase
	{
		/// <summary>
		/// 	The version-management entity context.
		/// </summary>
		readonly VersionManagementEntities _entities;

		/// <summary>
		/// 	Create a new <see cref="VersionsController"/>.
		/// </summary>
		/// <param name="entities">
		///		The version-management entity context.
		/// </param>
		public VersionsController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));

			_entities = entities;
		}

		/// <summary>
		/// 	Get the version corresponding to the specified combination of product, release, and commit Id. 
		/// </summary>
		/// <param name="productName">
		///		The product name.
		/// </param>
		/// <param name="releaseName">
		///		The release name.
		/// </param>
		/// <param name="commitId">
		///		The version control commit Id.
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpGet("")]
		public IActionResult GetVersion(string productName = null, string releaseName = null, string commitId = null)
		{
			if (String.IsNullOrWhiteSpace(productName))
				return BadRequest("Must specify a valid product name.");

			if (String.IsNullOrWhiteSpace(releaseName))
				return BadRequest("Must specify a valid release name.");

			if (String.IsNullOrWhiteSpace(commitId))
				return BadRequest("Must specify a valid commit Id.");

			ReleaseVersion releaseVersion = _entities.GetReleaseVersion(productName, releaseName, commitId);
			if (releaseVersion != null)
			{
				return Ok(new
				{
					ProductName = productName,
					ReleaseName = releaseName,
					CommitId = commitId,
					Version = releaseVersion.ToSemanticVersion()
				});
			}
			
			return EntityNotFound(new
			{
				Message = $"No version has been allocated for commit '{commitId}' in release '{releaseName}' of product '{productName}'.",
				ProductName = productName,
				ReleaseName = releaseName,
				CommitId = commitId,
				ErrorCode = "EntityNotFound"
			});
		}

		/// <summary>
		/// 	Get or create the version corresponding to the specified combination of product, release, and commit Id. 
		/// </summary>
		/// <param name="productName">
		///		The product name.
		/// </param>
		/// <param name="releaseName">
		///		The release name.
		/// </param>
		/// <param name="commitId">
		///		The version control commit Id.
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		[HttpPost("")]
		public IActionResult CreateVersion(string productName = null, string releaseName = null, string commitId = null)
		{
			if (String.IsNullOrWhiteSpace(productName))
				return BadRequest("Must specify a valid product name.");

			if (String.IsNullOrWhiteSpace(releaseName))
				return BadRequest("Must specify a valid release name.");

			if (String.IsNullOrWhiteSpace(commitId))
				return BadRequest("Must specify a valid commit Id.");

			ReleaseVersion releaseVersion = _entities.GetOrCreateReleaseVersion(productName, releaseName, commitId);

			return Ok(new
			{
				ProductName = productName,
				ReleaseName = releaseName,
				CommitId = commitId,
				Version = releaseVersion.ToSemanticVersion()
			});
		}
	}
}
