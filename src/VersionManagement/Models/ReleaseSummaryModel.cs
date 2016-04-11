using System;

namespace DD.Cloud.VersionManagement.Models
{
	using DataAccess.Models;

	/// <summary>
	///		Index view model for a release.
	/// </summary>
	///
    public class ReleaseSummaryModel
		: ReleaseModel
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
		///		Create a new <see cref="ReleaseSummaryModel"/> from the specified <see cref="ReleaseData"/>.
		/// </summary>
		/// <param name="releaseData">
		///		The release persistence model.
		/// </param>
		/// <returns>
		///		The new <see cref="ReleaseSummaryModel"/>, or <c>null</c> if the <see cref="ReleaseData"/> is <c>null</c>.
		/// </returns>
		public static new ReleaseSummaryModel FromData(ReleaseData releaseData)
		{
			if (releaseData == null)
				return null;
				
			if (releaseData.Product == null)
				throw new ArgumentException("Release was not loaded with its related product.", nameof(releaseData));
				
			if (releaseData.VersionRange == null)
				throw new ArgumentException("Release was not loaded with its related version range.", nameof(releaseData));

			return new ReleaseSummaryModel
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
    }
}
