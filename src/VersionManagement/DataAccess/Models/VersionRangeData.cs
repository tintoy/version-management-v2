using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
    [Table("VersionRange")]
	public sealed class VersionRangeData
	{
		public VersionRangeData()
		{
		}

		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[DefaultValue(VersionComponent.Build)]
		public VersionComponent IncrementBy { get; set; } = VersionComponent.Build;

		[Required]
		public int StartVersionMajor { get; set; }

		[Required]
		public int StartVersionMinor { get; set; }

		[Required]
		public int StartVersionBuild { get; set; }

		[Required]
		public int StartVersionRevision { get; set; }

		[Required]
		public int NextVersionMajor { get; set; }

		[Required]
		public int NextVersionMinor { get; set; }

		[Required]
		public int NextVersionBuild { get; set; }

		[Required]
		public int NextVersionRevision { get; set; }

		[Required]
		public int EndVersionMajor { get; set; }

		[Required]
		public int EndVersionMinor { get; set; }

		[Required]
		public int EndVersionBuild { get; set; }

		[Required]
		public int EndVersionRevision { get; set; }

		public ICollection<ReleaseData> Releases { get; set; } = new HashSet<ReleaseData>();

		[NotMapped]
		public Version StartVersion
		{
			get
			{
				return new Version(
				  StartVersionMajor,
				  StartVersionMinor,
				  StartVersionBuild,
				  StartVersionRevision
				);
			}
			set
			{
				if (value != null)
				{
					StartVersionMajor = value.Major;
					StartVersionMinor = value.Minor;
					StartVersionBuild = value.Build;
					StartVersionRevision = value.Revision;
				}
				else
				{
					StartVersionMajor = 0;
					StartVersionMinor = 0;
					StartVersionBuild = 0;
					StartVersionRevision = 0;
				}
			}
		}

		[NotMapped]
		public Version NextVersion
		{
			get
			{
				return new Version(
				  NextVersionMajor,
				  NextVersionMinor,
				  NextVersionBuild,
				  NextVersionRevision
				);
			}
			set
			{
				if (value != null)
				{
					NextVersionMajor = value.Major;
					NextVersionMinor = value.Minor;
					NextVersionBuild = value.Build;
					NextVersionRevision = value.Revision;
				}
				else
				{
					NextVersionMajor = 0;
					NextVersionMinor = 0;
					NextVersionBuild = 0;
					NextVersionRevision = 0;
				}
			}
		}

		[NotMapped]
		public Version EndVersion
		{
			get
			{
				return new Version(
				  EndVersionMajor,
				  EndVersionMinor,
				  EndVersionBuild,
				  EndVersionRevision
				);
			}
			set
			{
				if (value != null)
				{
					EndVersionMajor = value.Major;
					EndVersionMinor = value.Minor;
					EndVersionBuild = value.Build;
					EndVersionRevision = value.Revision;
				}
				else
				{
					EndVersionMajor = 0;
					EndVersionMinor = 0;
					EndVersionBuild = 0;
					EndVersionRevision = 0;
				}
			}
		}

		public Version GetAndIncrement()
		{
			Version nextVersion = NextVersion;

			switch (IncrementBy)
			{
				case VersionComponent.Major:
				{
					if (NextVersionMajor >= EndVersionMajor)
						throw new VersionManagementException("The next major version number for range '{0}' is {1}, which would exceed the maximum value for this version range.", Name, NextVersionMajor);

					NextVersionMajor++;

					break;
				}
				case VersionComponent.Minor:
				{
					if (NextVersionMinor >= EndVersionMinor)
						throw new VersionManagementException("The next minor version number for range '{0}' is {1}, which would exceed the maximum value for this version range.", Name, NextVersionMinor);

					NextVersionMinor++;

					break;
				}
				case VersionComponent.Build:
				{
					if (NextVersionBuild >= EndVersionBuild)
						throw new VersionManagementException("The next build number for range '{0}' is {1}, which would exceed the maximum value for this version range.", Name, NextVersionBuild);

					NextVersionBuild++;

					break;
				}
				case VersionComponent.Revision:
				{
					if (NextVersionRevision >= EndVersionRevision)
						throw new VersionManagementException("The next revision number for range '{0}' is {1}, which would exceed the maximum value for this version range.", Name, EndVersionRevision);

					NextVersionRevision++;

					break;
				}
				default:
				{
					throw new InvalidOperationException($"InvalidOperationException value for VersionRange.IncrementBy: {IncrementBy}.");
				}
			}

			return nextVersion;
		}

		public override string ToString() => $"{Name} ({StartVersion}-{EndVersion}, Next={NextVersion})";
	}
}
