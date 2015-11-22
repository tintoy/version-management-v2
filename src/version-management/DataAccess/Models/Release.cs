namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	/// <summary>
	///		Represents a release of a product in the version-management database. 
	/// </summary>
	public class Release
	{
		/// <summary>
		///		The release Id. 
		/// </summary>
		public int Id { get; set; }
		
		/// <summary>
		///		The release name. 
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		///		The release's product Id. 
		/// </summary>
		public int ProductId { get; set; } 
		
		/// <summary>
		///		The release's product. 
		/// </summary>
		public Product Product { get; set; }
	}
}
