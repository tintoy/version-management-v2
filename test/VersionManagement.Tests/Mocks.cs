using Microsoft.EntityFrameworkCore;
using System;

namespace DD.Cloud.VersionManagement.Tests
{
    /// <summary>
    ///     Helper methods for mocks used by functional tests.
    /// </summary>
    public static class Mocks
	{
		/// <summary>
		/// 	Create an instance of the specified <see cref="DbContext"/> type.
		/// </summary>
		/// <typeparam name="TEntityContext">
		///		The type of entity context to create.
		/// </typeparam>
		/// <returns>
		///		The new <typeparamref name="TEntityContext"/> instance.
		/// </returns>
		public static TEntityContext CreateInMemoryDbContext<TEntityContext>()
			where TEntityContext : DbContext
		{
			DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<TEntityContext>();
			
			optionsBuilder.UseInMemoryDatabase();
			
			return
				(TEntityContext)Activator.CreateInstance(
					typeof(TEntityContext),
					optionsBuilder.Options
				);
		}
	}
}
