namespace DD.Cloud.VersionManagement.DataAccess.Models
{
	public class Release
	{
		public int Id { get; set; }
		
		public string Name { get; set; }
		
		public int ProductId { get; set; } 
		
		public Product Product { get; set; }
	}
}
