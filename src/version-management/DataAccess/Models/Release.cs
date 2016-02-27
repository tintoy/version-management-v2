	using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	using System;

	public class Release
	{
		public Release()
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
		public Product Product { get; set; }

		[Required]
		public int VersionRangeId { get; set; }

		public VersionRange VersionRange { get; set; }

		public ICollection<ProductVersion> BuildVersions { get; set; } = new HashSet<ProductVersion>();

		public ProductVersion AllocateBuildVersion(string commitId)
		{
			if (String.IsNullOrWhiteSpace(commitId))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'commitId'.", nameof(commitId));

			if (VersionRange == null)
				throw new InvalidOperationException("VersionRange is null.");

			Version nextVersion = VersionRange.Increment();

			ProductVersion buildVersion = new ProductVersion(this, commitId, nextVersion);
			BuildVersions.Add(buildVersion);

			return buildVersion;
		}
	}
}
