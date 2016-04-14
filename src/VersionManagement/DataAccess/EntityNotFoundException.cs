using System;

namespace DD.Cloud.VersionManagement.DataAccess
{
	/// <summary>
	/// 	Exception representing an entity that was not found.
	/// </summary>
	public sealed class EntityNotFoundException
		: VersionManagementException
	{
		/// <summary>
		///     Create a new <see cref="EntityNotFoundException"/>.
		/// </summary>
		/// <param name="entityId">
		///     The Id of the entity that was not found.
        /// </param>
		/// <param name="entityType">
		///     The type of entity that was not found.
        /// </param>
		public EntityNotFoundException(int entityId, string entityType)
			: base("{0} not found with Id {1}", entityType, entityId)
		{
			if (String.IsNullOrWhiteSpace(entityType))
				throw new ArgumentException($"'{entityType}' is not a valid entity type.", nameof(entityType));
				
			EntityId = entityId;
			EntityType = entityType;
		}
		
		/// <summary>
		/// 	The Id of the entity that was not found.
		/// </summary>
		public int EntityId { get; }
		
		/// <summary>
		///		 The type of entity that was not found.
		/// </summary>
		public string EntityType { get; }
		
		/// <summary>
		/// 	Create an <see cref="EntityNotFoundException"/> representing a product that was not found.
		/// </summary>
		/// <param name="productId">
		///		The Id of the product that was not found.
		/// </param>
		/// <returns>
		///		The configured exception.
		/// </returns>
		public static EntityNotFoundException Product(int productId)
		{
			return new EntityNotFoundException(productId,
				entityType: "Product"
			);
		}
		
		/// <summary>
		/// 	Create an <see cref="EntityNotFoundException"/> representing a release that was not found.
		/// </summary>
		/// <param name="releaseId">
		///		The Id of the release that was not found.
		/// </param>
		/// <returns>
		///		The configured exception.
		/// </returns>
		public static EntityNotFoundException Release(int releaseId)
		{
			return new EntityNotFoundException(releaseId,
				entityType: "Release"
			);
		}
		
		/// <summary>
		/// 	Create an <see cref="EntityNotFoundException"/> representing a version range that was not found.
		/// </summary>
		/// <param name="versionRangeId">
		///		The Id of the version range that was not found.
		/// </param>
		/// <returns>
		///		The configured exception.
		/// </returns>
		public static EntityNotFoundException VersionRange(int versionRangeId)
		{
			return new EntityNotFoundException(versionRangeId,
				entityType: "VersionRange"
			);
		}
	}
}