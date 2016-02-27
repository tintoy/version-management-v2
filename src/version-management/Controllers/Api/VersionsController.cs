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

		[HttpGet("{productName}/{releaseName}/{commitId}")]
		public IActionResult GetVersion(string productName, string releaseName, string commitId)
		{
			if (String.IsNullOrWhiteSpace(productName))
				return BadRequest("Must specify a valid product name.");

			if (String.IsNullOrWhiteSpace(releaseName))
				return BadRequest("Must specify a valid release name.");

			if (String.IsNullOrWhiteSpace(commitId))
				return BadRequest("Must specify a valid commit Id.");

			ProductVersion buildVersion = _entities.GetBuildVersion(productName, releaseName, commitId);
			if (buildVersion != null)
			{
				return Ok(new
				{
					ProductName = productName,
					ReleaseName = releaseName,
					CommitId = commitId,
					Version = buildVersion.ToSemanticVersion()
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

		[HttpPost("{productName}/{releaseName}/{commitId}")]
		public IActionResult CreateVersion(string productName, string releaseName, string commitId)
		{
			if (String.IsNullOrWhiteSpace(productName))
				return BadRequest("Must specify a valid product name.");

			if (String.IsNullOrWhiteSpace(releaseName))
				return BadRequest("Must specify a valid release name.");

			if (String.IsNullOrWhiteSpace(commitId))
				return BadRequest("Must specify a valid commit Id.");

			ProductVersion buildVersion = _entities.GetOrCreateBuildVersion(productName, releaseName, commitId);

			return Ok(new
			{
				ProductName = productName,
				ReleaseName = releaseName,
				CommitId = commitId,
				Version = buildVersion.ToSemanticVersion()
			});
		}

		HttpNotFoundObjectResult EntityNotFound<TBody>(TBody body)
		{
			Context.Response.Headers["X-ErrorCode"] = "EntityNotFound";

			return new HttpNotFoundObjectResult(body);
		}
	}
}
