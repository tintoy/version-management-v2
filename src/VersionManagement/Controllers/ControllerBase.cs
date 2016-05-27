using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DD.Cloud.VersionManagement.Controllers
{
	/// <summary>
	///		The base class for controllers.
	/// </summary>
	public class ControllerBase
		: Controller
    {
		/// <summary>
		///		Create a new controller.
		/// </summary>
		/// <param name="log">
		///		The controller's log facility.
		/// </param>
		public ControllerBase(ILogger log)
		{
			if (log == null)
				throw new ArgumentNullException(nameof(log));

			Log = log;
		}

		/// <summary>
		///		The controller's log facility.
		/// </summary>
		protected ILogger Log { get; }
    }
}
