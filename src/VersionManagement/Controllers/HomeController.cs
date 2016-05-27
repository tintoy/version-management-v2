using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DD.Cloud.VersionManagement.Controllers
{
	/// <summary>
	///		The default ("Home") controller.
	/// </summary>
	[Route("")]
	public class HomeController
		: ControllerBase
	{
		/// <summary>
		///		Create a new <see cref="HomeController"/>.
		/// </summary>
		/// <param name="log">
		///		The controller's log facility.
		/// </param>
		public HomeController(ILogger<HomeController> log)
			: base(log)
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
			return Content("This is the version-management application.", contentType: "text/plain");
		}
	}
}
