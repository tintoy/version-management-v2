using System;
using System.ComponentModel.DataAnnotations;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	public sealed class ReleaseVersionData
    {
		public ReleaseVersionData()
		{
		}

		public ReleaseVersionData(ReleaseData release, string commitId, Version nextVersion)
		{
			if (release == null)
				throw new ArgumentNullException(nameof(release));

			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			if (nextVersion == null)
				throw new ArgumentNullException(nameof(nextVersion));

			Release = release;
			ReleaseId = release.Id;
			CommitId = commitId;
			VersionMajor = nextVersion.Major;
			VersionMinor = nextVersion.Minor;
			VersionBuild = nextVersion.Build;
			VersionRevision = nextVersion.Revision;
			SpecialVersion = release.SpecialVersion;
		}

		public string CommitId { get; set; }

		public int ReleaseId { get; set; }

		[Required]
		public int VersionMajor { get; set; }

		[Required]
		public int VersionMinor { get; set; }
		
		[Required]
		public int VersionBuild { get; set; }

		[Required]
		public int VersionRevision { get; set; }

		[Required]
		public string SpecialVersion { get; set; }

		public ReleaseData Release { get; set; }

		public string ToSemanticVersion()
		{
			return String.Format("{0}-{1}",
				new Version(VersionMajor, VersionMinor, VersionBuild, VersionRevision),
				SpecialVersion
			);
		}
	}
}
