using System;
using Microsoft.Data.Entity;

namespace DD.Cloud.VersionManagement.DataAccess
{
	using Models;

    public sealed class VersionManagementEntities
		: DbContext
	{
		public VersionManagementEntities()
		{
		}
		
		public DbSet<Product> Products { get; set; }
		
		public DbSet<Release> Releases { get; set; }
        
        public DbSet<VersionRange> VersionRanges { get; set; }
	}
}