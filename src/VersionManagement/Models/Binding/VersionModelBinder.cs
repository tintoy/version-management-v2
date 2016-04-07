using Microsoft.AspNet.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace DD.Cloud.VersionManagement.Models.Binding
{
	/// <summary>
	///		Model binder for <see cref="Version"/>s.
	/// </summary>
	public class VersionModelBinder
		: IModelBinder
    {
		/// <summary>
		///		Create a new <see cref="VersionModelBinder"/>.
		/// </summary>
		public VersionModelBinder()
		{
		}

		/// <summary>
		///		Asynchronously bind to a particular model.
		/// </summary>
		/// <param name="bindingContext">
		///		Contextual information about the model being bound.
		/// </param>
		/// <returns>
		///		A <see cref="ModelBindingResult"/> representing the result of the model-binding process.
		/// </returns>
		/// <remarks>
		///		A non <c>null</c> value indicates that the model binder was able to handle the request.
		///		A <c>null</c> return value means that the model binder was not able to handle the request.
		///		Returning <c>null</c> ensures that subsequent model binders are run.
		/// </remarks>
		public Task<ModelBindingResult> BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
				throw new ArgumentNullException(nameof(bindingContext));

			if (bindingContext.ModelType != typeof(Version))
				throw new InvalidOperationException("This model binder is only");

			string modelValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
			if (String.IsNullOrWhiteSpace(modelValue))
			{
				return Task.FromResult(
					ModelBindingResult.Success(bindingContext.ModelName, null)
				);
			}

			Version version;
			if (Version.TryParse(modelValue, out version))
			{
				return Task.FromResult(
					ModelBindingResult.Success(bindingContext.ModelName, version)
				);
			}

			bindingContext.ModelState.AddModelError(bindingContext.ModelName,
				$"'{modelValue}' is not a valid version number."
			);

			return Task.FromResult(
				ModelBindingResult.Failed(bindingContext.ModelName)
			);
		}
    }
}
