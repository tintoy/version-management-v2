using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	/// <summary>
	/// 	Persistence model for a product.
	/// </summary>
	/// <remarks>
	///		A product is used to group related releases together.
	/// </remarks>
	public sealed class ProductData
	{
		/// <summary>
		/// 	The product Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 	The product name.
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// 	The product's associated releases (if any).
		/// </summary>
		public ICollection<ReleaseData> Releases { get; set; } = new HashSet<ReleaseData>();
	}
}
