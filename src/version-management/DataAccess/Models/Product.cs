using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	public sealed class Product
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public ICollection<Release> Releases { get; set; } = new HashSet<Release>();
	}
}
