using System;
using System.Collections.Generic;
using System.Linq;

namespace DD.Cloud.VersionManagement.Models
{
	using DataAccess.Models;

	/// <summary>
	///		Display model for a release.
	/// </summary>
	///
    public class ReleaseDisplayModel
		: ReleaseEditModel
    {
		/// <summary>
		///		The name of the release's associated product.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		///		The name of the release's associated version range.
		/// </summary>
		public string VersionRangeName { get; set; }

		/// <summary>
		///		Create a new <see cref="ReleaseDisplayModel"/> from the specified <see cref="ReleaseData"/>.
		/// </summary>
		/// <param name="releaseData">
		///		The release persistence model.
		/// </param>
		/// <returns>
		///		The new <see cref="ReleaseDisplayModel"/>, or <c>null</c> if the <see cref="ReleaseData"/> is <c>null</c>.
		/// </returns>
		public static new ReleaseDisplayModel FromData(ReleaseData releaseData)
		{
			if (releaseData == null)
				return null;
				
			if (releaseData.Product == null)
				throw new ArgumentException("Release was not loaded with its related product.", nameof(releaseData));
				
			if (releaseData.VersionRange == null)
				throw new ArgumentException("Release was not loaded with its related version range.", nameof(releaseData));

			return new ReleaseDisplayModel
			{
				Id = releaseData.Id,
				Name = releaseData.Name,
				ProductId = releaseData.ProductId,
				ProductName = releaseData.Product.Name,
				VersionRangeId = releaseData.VersionRangeId,
				VersionRangeName = releaseData.VersionRange.Name,
				SpecialVersion = releaseData.SpecialVersion
			};
		}
		
		/// <summary>
		///		Create a sequence of <see cref="ReleaseDisplayModel"/>s from the specified <see cref="ReleaseSummaryData"/>.
		/// </summary>
		/// <param name="releaseData">
		///		The product persistence models.
		/// </param>
		/// <returns>
		///		A sequence of <see cref="ReleaseDisplayModel"/>.
		/// </returns>
		public static new IEnumerable<ReleaseDisplayModel> FromData(IEnumerable<ReleaseData> releaseData)
		{
			if (releaseData == null)
				throw new ArgumentNullException(nameof(releaseData));
				
			return releaseData.Select(
				data => FromData(data)
			);
		}
    }
}
