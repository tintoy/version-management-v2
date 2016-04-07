using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using System;
using System.Linq;

namespace DD.Cloud.VersionManagement.DataAccess
{
	using Models;

	/// <summary>
	///		Helper functions for generating select lists.
	/// </summary>
	public static class SelectLists
    {
		/// <summary>
		///		Create a <see cref="SelectList"/> containing all possible <see cref="VersionComponent"/>s.
		/// </summary>
		/// <param name="selectedVersionComponent">
		///		The currently-selected version component (if any).
		/// </param>
		/// <returns>
		///		The new <see cref="SelectList"/>.
		/// </returns>
		public static SelectList VersionComponents(VersionComponent? selectedVersionComponent = null)
		{
			var versionComponentOptions =
				Enum.GetValues(typeof(VersionComponent))
					.Cast<VersionComponent>()
					.Where(versionComponent => versionComponent != VersionComponent.Unknown)
					.Select(versionComponent => new
					{
						Name = versionComponent.ToString(),
						Value = versionComponent
					})
					.ToArray();

			return new SelectList(versionComponentOptions,
				dataValueField: "Value",
				dataTextField: "Name",
				selectedValue: selectedVersionComponent
			);
		}

		/// <summary>
		///		Create a <see cref="SelectList"/> containing all products.
		/// </summary>
		/// <param name="versionManagementEntities">
		///		The version management entity context.
		/// </param>
		/// <param name="selectedProductId">
		///		The Id of the currently-selected product (if any).
		/// </param>
		/// <returns>
		///		The new <see cref="SelectList"/>.
		/// </returns>
		/// <remarks>
		///		TODO: Replace use of <see cref="VersionManagementEntities"/> with <see cref="IVersionManagementData"/>.
		/// </remarks>
		public static SelectList Products(VersionManagementEntities versionManagementEntities, int? selectedProductId = null)
		{
			if (versionManagementEntities == null)
				throw new ArgumentNullException(nameof(versionManagementEntities));

			ProductData[] products =
				versionManagementEntities.Products.AsNoTracking()
					.OrderBy(product => product.Name)
					.ToArray();

			return new SelectList(products,
				dataValueField: "Id",
				dataTextField: "Name",
				selectedValue: selectedProductId
			);
		}

		/// <summary>
		///		Create a <see cref="SelectList"/> containing all known version ranges.
		/// </summary>
		/// <param name="versionManagementEntities">
		///		The version management entity context.
		/// </param>
		/// <param name="selectedVersionRangeId">
		///		The Id of the currently-selected version range (if any).
		/// </param>
		/// <returns>
		///		The new <see cref="SelectList"/>.
		/// </returns>
		/// <remarks>
		///		TODO: Replace use of <see cref="VersionManagementEntities"/> with <see cref="IVersionManagementData"/>.
		/// </remarks>
		public static SelectList VersionRanges(VersionManagementEntities versionManagementEntities, int? selectedVersionRangeId = null)
		{
			if (versionManagementEntities == null)
				throw new ArgumentNullException(nameof(versionManagementEntities));

			VersionRangeData[] versionRanges =
				versionManagementEntities.VersionRanges.AsNoTracking()
					.OrderBy(versionRange => versionRange.Name)
					.ToArray();

			return new SelectList(versionRanges,
				dataValueField: "Id",
				dataTextField: "Name",
				selectedValue: selectedVersionRangeId
			);
		}
	}
}
