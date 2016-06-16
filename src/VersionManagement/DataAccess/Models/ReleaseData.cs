using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
    [Table("Release")]
	public class ReleaseData
	{
		public ReleaseData()
		{
		}

		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string SpecialVersion { get; set; }

		[Required]
		[ForeignKey("Product")]
		public int ProductId { get; set; }

		[Required]
		public ProductData Product { get; set; }

		[Required]
		public int VersionRangeId { get; set; }

		public VersionRangeData VersionRange { get; set; }

		public ICollection<ReleaseVersionData> BuildVersions { get; set; } = new HashSet<ReleaseVersionData>();

		public ReleaseVersionData AllocateReleaseVersion(string commitId)
		{
			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			if (VersionRange == null)
				throw new InvalidOperationException("VersionRange is null.");

			Version nextVersion = VersionRange.GetAndIncrement();

			ReleaseVersionData releaseVersion = new ReleaseVersionData(this, commitId, nextVersion);
			BuildVersions.Add(releaseVersion);

			return releaseVersion;
		}
	}
}
