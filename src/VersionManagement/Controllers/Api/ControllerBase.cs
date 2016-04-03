using Microsoft.AspNet.Mvc;
using System.Web.Http;

namespace DD.Cloud.VersionManagement.Controllers.Api
{
	/// <summary>
	/// 	The base class for version-management API controllers.
	/// </summary>
	public abstract class ControllerBase
		: ApiController
	{
		/// <summary>
		/// 	Create a new version-management API controller.
		/// </summary>
		protected ControllerBase()
		{
		}
		
		/// <summary>
		///		Create an action result representing an entity that was not found.
		/// </summary>
		/// <typeparam name="TBody">
		///		The response body type.
		/// </typeparam>
		/// <param name="body">
		///		The response body.
		/// </param>
		/// <returns>
		///		The action result.
		/// </returns>
		protected virtual HttpNotFoundObjectResult EntityNotFound<TBody>(TBody body)
		{
			Context.Response.Headers["X-ErrorCode"] = "EntityNotFound";
			
			// TODO: Add X-EntityType header.

			return new HttpNotFoundObjectResult(body);
		}
	}
}