using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	public class Release
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string VersionSuffix { get; set; }

		[Required]
		[ForeignKey("Product")]
		public int ProductId { get; set; }

		[Required]
		public Product Product { get; set; }

		public int VersionRangeId { get; set; }

		public VersionRange VersionRange { get; set; }
	}
}
