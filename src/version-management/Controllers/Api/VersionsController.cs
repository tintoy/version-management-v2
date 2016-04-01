using Microsoft.AspNet.Mvc;
using System;
using System.Web.Http;

namespace DD.Cloud.VersionManagement.Controllers.Api
{
	using DataAccess;
	using DataAccess.Models;

	[Route("api/v2/versions")]
	public class VersionsController
	  : ApiController
	{
		readonly VersionManagementEntities _entities;

		public VersionsController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));

			_entities = entities;
		}

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

		HttpNotFoundObjectResult EntityNotFound<TBody>(TBody body)
		{
			Context.Response.Headers["X-ErrorCode"] = "EntityNotFound";

			return new HttpNotFoundObjectResult(body);
		}
	}
}
