namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	/// <summary>
	///		Represents a product in the version-management database. 
	/// </summary>
	public sealed class Product
	{
		/// <summary>
		///		The product Id. 
		/// </summary>
		public int Id { get; set; }
		
		/// <summary>
		///		The product name. 
		/// </summary>
		public string Name { get; set; }
	}
}