using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	/// <summary>
	/// 	Persistence model for a release version allocated to a specific commit. 
	/// </summary>
    [Table("ReleaseVersion")]    
	public sealed class ReleaseVersionData
    {
		/// <summary>
		/// 	Create a new product / release version.
		/// </summary>
		public ReleaseVersionData()
		{
		}

		/// <summary>
		/// 	Create a new release version.
		/// </summary>
		/// <param name="release">
		///		The release for which the version was allocated.
		/// </param>
		/// <param name="commitId">
		///		The commit Id for which the version was allocated.
		/// </param>
		/// <param name="version">
		///		The version.
		/// </param>
		public ReleaseVersionData(ReleaseData release, string commitId, Version version)
		{
			if (release == null)
				throw new ArgumentNullException(nameof(release));

            if (release.VersionRange == null)
                throw new ArgumentException("Release.VersionRange cannot be null.", nameof(release));

			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			if (version == null)
				throw new ArgumentNullException(nameof(version));

			Release = release;
            FromVersionRange = release.VersionRange;
            FromVersionRangeId = release.VersionRangeId;
			ReleaseId = release.Id;
			CommitId = commitId;
			VersionMajor = version.Major;
			VersionMinor = version.Minor;
			VersionBuild = version.Build;
			VersionRevision = version.Revision;
			SpecialVersion = release.SpecialVersion;
            
		}

		/// <summary>
		/// 	The Id of the release for which the version was allocated.
		/// </summary>
		public int ReleaseId { get; set; }

		/// <summary>
		/// 	The commit Id for which the version was allocated.
		/// </summary>
		public string CommitId { get; set; }

		/// <summary>
		/// 	The major version number.
		/// </summary>
		[Required]
		public int VersionMajor { get; set; }

		/// <summary>
		/// 	The minor version number.
		/// </summary>
		[Required]
		public int VersionMinor { get; set; }
		
		/// <summary>
		/// 	The build number.
		/// </summary>
		[Required]
		public int VersionBuild { get; set; }

		/// <summary>
		/// 	The revision number.
		/// </summary>
		[Required]
		public int VersionRevision { get; set; }

		/// <summary>
		/// 	The special version tag (if any).
		/// </summary>
		[Required]
		public string SpecialVersion { get; set; }

		/// <summary>
		/// 	The release for which the version was allocated.
		/// </summary>
        [Required]
		public ReleaseData Release { get; set; }

        /// <summary>
		/// 	The version range from which the version was allocated.
		/// </summary>
        [Required]
        public VersionRangeData FromVersionRange { get; set; }

        /// <summary>
		/// 	The Id of the version range from which the version was allocated.
		/// </summary>
        [Required]
		public int FromVersionRangeId { get; set; }

		/// <summary>
		/// 	Convert the release version information to a semantic version (e.g. "1.0.0-alpha1").
		/// </summary>
		public string ToSemanticVersion()
		{
			return String.Format("{0}-{1}",
				new Version(VersionMajor, VersionMinor, VersionBuild, VersionRevision),
				SpecialVersion
			);
		}
	}
}
