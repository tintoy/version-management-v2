using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DD.Cloud.VersionManagement.Controllers.Api
{
	using DataAccess;
	using DataAccess.Models;

	[Route("api/v1/releases")]
	public class ReleasesController
		: ApiController
	{
		readonly VersionManagementEntities _entities;

		public ReleasesController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));

			_entities = entities;
		}

		[HttpGet("")]
		public IActionResult GetAllReleases()
		{
			Release[] Releases = _entities.Releases.ToArray();

			return Ok(Releases);
		}

		[HttpGet("{id:int?}")]
		public IActionResult GetReleaseById([Required] int id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Release matchingRelease = _entities.Releases.FirstOrDefault(
				Release => Release.Id == id
			);
			if (matchingRelease != null)
				return Ok(matchingRelease);

			return NotFound();
		}

		[HttpGet("")]
		public IActionResult GetReleaseByName([Required] string productName, [Required] string name)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Release matchingRelease = _entities.Releases.FirstOrDefault(
				release => release.Name == name && release.Product.Name == productName
			);
			if (matchingRelease != null)
				return Ok(matchingRelease);

			return NotFound();
		}

		[HttpPost("")]
		public async Task<IActionResult> Create([Required] int productId, [Required] string name = null)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Release Release = _entities.Releases.FirstOrDefault(
				existingRelease => existingRelease.Name == name && existingRelease.ProductId == productId
			);
			if (Release != null)
			{
				Context.Response.Headers.Add("X-Reason",
					$"Release named '{name}' already exists for ProductId '{productId}'."
				);

				return Conflict();
			}

			Release = new Release
			{
				Name = name,
				ProductId = productId
			};
			_entities.Releases.Add(Release);
			await _entities.SaveChangesAsync();

			return Ok(Release);
		}

		[HttpPut("{id:int?}")]
		public async Task<IActionResult> Update([Required] int id, [Required] string name = null)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Release existingRelease = _entities.Releases.FirstOrDefault(
				matchingRelease => matchingRelease.Id == id
			);
			if (existingRelease == null)
				return NotFound();

			existingRelease.Name = name;
			await _entities.SaveChangesAsync();

			return Ok(existingRelease);
		}

		[HttpDelete("{id:int?}")]
		public async Task<IActionResult> Delete([Required, FromUri] int id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Release matchingRelease = _entities.Releases.FirstOrDefault(
				Release => Release.Id == id
			);
			if (matchingRelease == null)
				return NotFound();

			_entities.Releases.Remove(matchingRelease);
			await _entities.SaveChangesAsync();

			return Ok();
		}
	}
}
