using System;
using DD.Cloud.VersionManagement.DataAccess;
using Microsoft.AspNet.Mvc;

namespace DD.Cloud.VersionManagement.Controllers
{
	/// <summary>
    ///		Default ("Home") controller for the version-management web application.
    /// </summary>
	[Route("")]
	public class HomeController
		: Controller
	{
		/// <summary>
		///		The version-management entity context.
		/// </summary>
		readonly VersionManagementEntities _entities;
		
		/// <summary>
		///		Create a new instance of the Home controller.
		/// </summary>
		/// <param name="entities">
		///		The version-management entity context.
		/// </param>
		public HomeController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new ArgumentNullException("entities");
				
			_entities = entities;
		}
		
		/// <summary>
		///		Display the application home page.
		/// </summary>
		[Route("")]
		public IActionResult Index()
		{
			return Content("This is the version-management application.");
		}
	}
}