using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace DD.Cloud.VersionManagement.Models
{
	using Binding;
	using DataAccess.Models;

	/// <summary>
	///		View model for a version range.
	/// </summary>
    public class VersionRangeModel
    {
		/// <summary>
		///		The version range Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///		The version range name.
		/// </summary>
		[Required(ErrorMessage = "Version range must have a name.")]
		public string Name { get; set; }

		/// <summary>
		///		The first available version in the range.
		/// </summary>
		[ModelBinder(BinderType = typeof(VersionModelBinder))]
		[Required(ErrorMessage = "Version range must have a starting version")]
		public Version StartVersion { get; set; }

		/// <summary>
		///		The last available version in the range.
		/// </summary>
		[ModelBinder(BinderType = typeof(VersionModelBinder))]
		[Required(ErrorMessage = "Version range must have an ending version")]
		public Version EndVersion { get; set; }

		/// <summary>
		///		The next available version in the range.
		/// </summary>
		[ModelBinder(BinderType = typeof(VersionModelBinder))]
		[Required(ErrorMessage = "Version range must have a next version")]
		public Version NextVersion { get; set; }

		/// <summary>
		///		The version component by which the range is incremented when allocating a new version.
		/// </summary>
		public VersionComponent IncrementBy { get; set; }

		/// <summary>
		///		Convert the <see cref="VersionRangeModel"/> to a new <see cref="VersionRangeData"/>.
		/// </summary>
		/// <returns>
		///		The new <see cref="VersionRangeData"/>.
		/// </returns>
		public VersionRangeData ToData()
		{
			return new VersionRangeData
			{
				Id = Id,
				Name = Name,
				StartVersion = StartVersion,
				EndVersion = EndVersion,
				NextVersion = NextVersion,
				IncrementBy = IncrementBy
			};
		}

		/// <summary>
		///		Update existing <see cref="VersionRangeData"/> from the <see cref="VersionRangeModel"/>.
		/// </summary>
		/// <param name="versionRangeData">
		///		The <see cref="VersionRangeData"/> to update.
		/// </param>
		public void ToData(VersionRangeData versionRangeData)
		{
			if (versionRangeData == null)
				throw new ArgumentNullException(nameof(versionRangeData));
			
			if (versionRangeData.Id != Id)
				throw new InvalidOperationException($"Cannot update version range data for version range {versionRangeData.Id} from model for version range {Id} (Ids do not match).");

			versionRangeData.Name = Name;
			versionRangeData.StartVersion = StartVersion;
			versionRangeData.EndVersion = EndVersion;
			versionRangeData.NextVersion = NextVersion;
			versionRangeData.IncrementBy = IncrementBy;
		}

		/// <summary>
		///		Create a new <see cref="VersionRangeModel"/> from the specified <see cref="VersionRangeData"/>.
		/// </summary>
		/// <param name="versionRangeData">
		///		The version range persistence model.
		/// </param>
		/// <returns>
		///		The new <see cref="VersionRangeModel"/>, or <c>null</c> if the <see cref="VersionRangeData"/> is <c>null</c>.
		/// </returns>
		public static VersionRangeModel FromData(VersionRangeData versionRangeData)
		{
			if (versionRangeData == null)
				return null;

			return new VersionRangeModel
			{
				Id = versionRangeData.Id,
				Name = versionRangeData.Name,
				StartVersion = versionRangeData.StartVersion,
				EndVersion = versionRangeData.EndVersion,
				NextVersion = versionRangeData.NextVersion,
				IncrementBy = versionRangeData.IncrementBy
			};
		}
    }
}
