using System.Collections.Generic;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	/// <summary>
	///		Represents a product in the version-management database. 
	/// </summary>
	public sealed class Product
	{
		public int Id { get; set; }
		
		public string Name { get; set; }
		
		public ICollection<Release> Releases { get; set; } 
	}
}