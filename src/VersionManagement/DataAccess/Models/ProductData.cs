using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	public sealed class ProductData
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public ICollection<ReleaseData> Releases { get; set; } = new HashSet<ReleaseData>();
	}
}
