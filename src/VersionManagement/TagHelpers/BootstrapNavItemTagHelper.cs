using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.TagHelpers;
using System;
using System.Linq;

namespace DD.Cloud.VersionManagement.TagHelpers
{
	/// <summary>
	///		Tag helper for Bootstrap navigation items that marks an item as active if its target controller and action are the current controller and action.
	/// </summary>
	[HtmlTargetElement("li", Attributes = "bootstrap-nav-action")]
	[HtmlTargetElement("li", Attributes = "bootstrap-nav-controller")]
    public class BootstrapNavItemTagHelper
		: TagHelper
    {
		/// <summary>
		///		Create a new <see cref="BootstrapNavItemTagHelper"/>.
		/// </summary>
		public BootstrapNavItemTagHelper()
		{
		}

		/// <summary>
		///		Contextual information for the current view.
		/// </summary>
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		/// <summary>
		///		The controller that must be current in order for the navigation item to be marked as active.
		/// </summary>
		[HtmlAttributeName("bootstrap-nav-controller")]
		public string Controller { get; set; }

		/// <summary>
		///		The action that must be current in order for the navigation item to be marked as active.
		/// </summary>
		[HtmlAttributeName("bootstrap-nav-action")]
		public string Action { get; set; }

		/// <summary>
		///		Process the tag.
		/// </summary>
		/// <param name="context">
		///		Contextual information about the tag being generated.
		/// </param>
		/// <param name="output">
		///		The tag helper output.
		/// </param>
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			if (output == null)
				throw new ArgumentNullException(nameof(output));

			if (ShouldBeActive())
				MakeActive(output);
		}

		/// <summary>
		///		Should the navigation item be marked as active?
		/// </summary>
		/// <returns>
		///		<c>true</c>, if the navigation item should be marked as active; otherwise, <c>false</c>.
		/// </returns>
		bool ShouldBeActive()
		{
			string currentController = ViewContext.RouteData.Values["controller"]?.ToString();
			string currentAction = ViewContext.RouteData.Values["action"]?.ToString();

			if (!String.IsNullOrWhiteSpace(Controller) && !String.IsNullOrWhiteSpace(Action))
				return String.Equals(Controller, currentController, StringComparison.OrdinalIgnoreCase) && String.Equals(Action, currentAction, StringComparison.OrdinalIgnoreCase);

			if (!String.IsNullOrWhiteSpace(Action))
				return String.Equals(Action, currentAction, StringComparison.OrdinalIgnoreCase);

			if (!String.IsNullOrWhiteSpace(Controller))
				return String.Equals(Controller, currentController, StringComparison.OrdinalIgnoreCase);

			return false;
		}

		/// <summary>
		///		Mark the navigation item as active.
		/// </summary>
		/// <param name="output">
		///		The tag helper's output.
		/// </param>
		void MakeActive(TagHelperOutput output)
		{
			if (output == null)
				throw new ArgumentNullException(nameof(output));

			TagHelperAttribute classAttribute = output.Attributes.FirstOrDefault(
				attribute => attribute.Name == "class"
			);
			if (classAttribute != null)
			{
				string classes = classAttribute.Value?.ToString();
				if (!String.IsNullOrWhiteSpace(classes))
					classes += " active";
				else
					classes = "active";

				classAttribute.Value = classes;
			}
			else
			{
				classAttribute = new TagHelperAttribute("class", "active");
				output.Attributes.Add(classAttribute);
			}
		}
	}
}
