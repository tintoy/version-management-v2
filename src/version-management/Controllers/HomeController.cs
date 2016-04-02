using System;
using DD.Cloud.VersionManagement.DataAccess;
using Microsoft.AspNet.Mvc;

namespace DD.Cloud.VersionManagement.Controllers
{
	/// <summary>
	///		The default ("Home") controller.
	/// </summary>
	[Route("")]
	public class HomeController
		: Controller
	{
		/// <summary>
		///		Create a new <see cref="HomeController"/>.
		/// </summary>
		public HomeController()
		{
		}

		/// <summary>
		///		Show the version-management home page.
		/// </summary>
		/// <returns>
		///		An action result that renders the home page.
		/// </returns>
		[Route("")]
		public IActionResult Index()
		{
			return Content("This is the version-management application.");
		}
	}
}
