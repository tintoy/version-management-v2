using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	public class VersionRange
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

		public ICollection<Release> Releases { get; set; }

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

		public Version Increment()
		{
			switch (IncrementBy)
			{
				case VersionComponent.Major:
				{
					NextVersionMajor++;

					break;
				}
				case VersionComponent.Minor:
				{
					NextVersionMinor++;

					break;
				}
				case VersionComponent.Build:
				{
					NextVersionBuild++;

					break;
				}
				case VersionComponent.Revision:
				{
					NextVersionRevision++;

					break;
				}
				default:
				{
				  throw new InvalidOperationException($"InvalidOperationException value for VersionRange.IncrementBy: {IncrementBy}.");
				}
			}

			return NextVersion;
		}

		public override string ToString() => $"{Name} ({StartVersion}-{EndVersion}, Next={NextVersion})";
	}
}
