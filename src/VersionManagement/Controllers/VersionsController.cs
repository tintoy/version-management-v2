using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace DD.Cloud.VersionManagement.Controllers
{
    using DataAccess;
    using DataAccess.Models;
    using Models;

    /// <summary>
    ///		The UI controller for allocated version information.
    /// </summary>
    [Route("versions")]
	public class VersionsController
		: ControllerBase
	{
		/// <summary>
		///     The version-management data access facility.
		/// </summary>
		readonly IVersionManagementData     _data;
		
		/// <summary>
		///     The version-management entity context.
		/// </summary>
		readonly VersionManagementEntities	_entities;
		
		/// <summary>
		///		Create a new <see cref="VersionsController"/>.
		/// </summary>
		/// <param name="data">
		///		The version-management data access facility.
		/// </param>
		/// <param name="entities">
		///		The version-management entity context.
		/// </param>
		/// <param name="log">
		///		The controller's log facility.
		/// </param>
		public VersionsController(IVersionManagementData data, VersionManagementEntities entities, ILogger<VersionsController> log)
			: base(log)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));
				
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));
				
			_data = data;
			_entities = entities;
		}
		
		/// <summary>
		///		Display all versions allocated for a specific release.
		/// </summary>
		/// <param name="commitId">
		///		The commit Id.
		/// </param>
		/// <param name="productId">
		///		An optional product Id used to filter the versions.
		/// </param>
		[HttpGet("{commitId}")]
		public IActionResult GetVersionsByCommit(string commitId, int? productId = null)
		{
			if (String.IsNullOrWhiteSpace(commitId))
			{
				ModelState.AddModelError(nameof(commitId),
					"Must supply a valid commit Id."
				);
				
				return BadRequest(ModelState);
			}
			
			ViewBag.CommitId = commitId;
			
			var versionQuery = 
				_entities.ReleaseVersions
					.Include(releaseVersion => releaseVersion.Release.Product)
					.Where(releaseVersion => releaseVersion.CommitId == commitId);
			
			if (productId.HasValue)
			{
				ProductModel product = _data.GetProductById(productId.Value);
				if (product == null)
				{
					ModelState.AddModelError(nameof(productId),
						$"No product was found with Id '{productId.Value}'."
					);
					
					return BadRequest(ModelState);
				}
				
				ViewBag.ProductName = product.Name;
				
				versionQuery = versionQuery.Where(
					releaseVersion => releaseVersion.Release.ProductId == productId.Value
				);
			}
			else
				ViewBag.ProductName = null;
			
			ReleaseVersionData[] matchingVersions = versionQuery.ToArray();
				
			return View("VersionsByCommit", matchingVersions);
		}
	}
}