﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DD.Cloud.VersionManagement.Models
{
    using DataAccess.Models;

    /// <summary>
    ///		Create / edit view model for a release.
    /// </summary>
    public class ReleaseEditModel
    {
		/// <summary>
		///		The release Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///		The release name.
		/// </summary>
		[Required(ErrorMessage = "Release must have a name.")]
		public string Name { get; set; }

		/// <summary>
		///		The Id of the release's associated product.
		/// </summary>
		[Required(ErrorMessage = "Release must have an associated product.")]
		public int? ProductId { get; set; }

		/// <summary>
		///		The Id of the release's associated version range.
		/// </summary>
		[Required(ErrorMessage = "Release must have an associated version range.")]
		public int? VersionRangeId { get; set; }

		/// <summary>
		///		The release's special version tag (if any).
		/// </summary>
		public string SpecialVersion { get; set; }

		/// <summary>
		///		Convert the <see cref="ReleaseEditModel"/> to a new <see cref="ReleaseData"/>.
		/// </summary>
		/// <returns>
		///		The new <see cref="ReleaseData"/>.
		/// </returns>
		public ReleaseData ToData()
		{
			if (!ProductId.HasValue)
				throw new InvalidOperationException("Release does not have associated product Id.");

			if (!VersionRangeId.HasValue)
				throw new InvalidOperationException("Release does not have associated version range Id.");

			return new ReleaseData
			{
				Id = Id,
				Name = Name,
				ProductId = ProductId.Value,
				VersionRangeId = VersionRangeId.Value,
				SpecialVersion = SpecialVersion
			};
		}

		/// <summary>
		///		Update existing <see cref="ReleaseData"/> from the <see cref="ReleaseEditModel"/>.
		/// </summary>
		/// <param name="releaseData">
		///		The <see cref="ReleaseData"/> to update.
		/// </param>
		public virtual void ToData(ReleaseData releaseData)
		{
			if (releaseData == null)
				throw new ArgumentNullException(nameof(releaseData));

			if (releaseData.Id != Id)
				throw new InvalidOperationException($"Cannot update release data for release {releaseData.Id} from model for release {Id} (Ids do not match).");

			if (!ProductId.HasValue)
				throw new InvalidOperationException("Release does not have associated product Id.");

			if (!VersionRangeId.HasValue)
				throw new InvalidOperationException("Release does not have associated version range Id.");

			releaseData.Name = Name;
			releaseData.ProductId = ProductId.Value;
			releaseData.VersionRangeId = VersionRangeId.Value;
			releaseData.SpecialVersion = SpecialVersion;
		}

		/// <summary>
		///		Create a new <see cref="ReleaseEditModel"/> from the specified <see cref="ReleaseData"/>.
		/// </summary>
		/// <param name="releaseData">
		///		The release persistence model.
		/// </param>
		/// <returns>
		///		The new <see cref="ReleaseEditModel"/>, or <c>null</c> if the <see cref="ReleaseData"/> is <c>null</c>.
		/// </returns>
		public static ReleaseEditModel FromData(ReleaseData releaseData)
		{
			if (releaseData == null)
				return null;

			return new ReleaseEditModel
			{
				Id = releaseData.Id,
				Name = releaseData.Name,
				ProductId = releaseData.ProductId,
				VersionRangeId = releaseData.VersionRangeId,
				SpecialVersion = releaseData.SpecialVersion
			};
		}
		
		/// <summary>
		///		Create a sequence of <see cref="ReleaseEditModel"/>s from the specified <see cref="ReleaseData"/>s.
		/// </summary>
		/// <param name="releaseData">
		///		The release persistence models.
		/// </param>
		/// <returns>
		///		A sequence of <see cref="ReleaseEditModel"/>s.
		/// </returns>
		public static IEnumerable<ReleaseEditModel> FromData(IEnumerable<ReleaseData> releaseData)
		{
			if (releaseData == null)
				throw new ArgumentNullException(nameof(releaseData));
				
			return releaseData.Select(
				data => FromData(data)
			);
		}
    }
}
