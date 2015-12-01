using System;
using DD.Cloud.VersionManagement.DataAccess;
using Microsoft.AspNet.Mvc;

namespace DD.Cloud.VersionManagement.Controllers
{
	[Route("")]
	public class HomeController
		: Controller
	{
		public HomeController(VersionManagementEntities entities)
		{
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));
		}

		[Route("")]
		public IActionResult Index()
		{
			return Content("This is the version-management application.");
		}
	}
}
