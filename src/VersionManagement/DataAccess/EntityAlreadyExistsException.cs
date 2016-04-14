using System;

namespace DD.Cloud.VersionManagement.DataAccess
{
    /// <summary>
	///     Exception representing an entity already existing.
	/// </summary>
	public sealed class EntityAlreadyExistsException
		: VersionManagementException
	{
        /// <summary>
		///     Create a new <see cref="EntityAlreadyExistsException"/>.
		/// </summary>
		/// <param name="entityType">
        ///     The type of entity that already exists.
        /// </param>
		/// <param name="messageOrFormat">
        ///     The exception message or message-format specifier.
        /// </param>
		/// <param name="formatArguments">
        ///     Optional format arguments.
        /// </param>
		public EntityAlreadyExistsException(string entityType, string messageOrFormat, params object[] formatArguments)
			: base(messageOrFormat, formatArguments)
		{
			if (String.IsNullOrWhiteSpace(entityType))
				throw new ArgumentException($"'{entityType}' is not a valid entity type.", nameof(entityType));

			EntityType = entityType;
		}
		
        /// <summary>
		///     The type of entity that already exists.
		/// </summary>
		public string EntityType { get; }
		
        /// <summary>
		///     Create an <see cref="EntityAlreadyExistsException"/> representing a release that already exists with the specified name and product.
		/// </summary>
		/// <param name="releaseName">
        ///     The name of the release that already exisexists
        /// </param>
		/// <param name="productName">
        ///     The name of the releases associated product.    
        /// </param>
		/// <returns>
        ///     The configured exception.
        /// </returns>
		public static EntityAlreadyExistsException ReleaseWithName(string releaseName, string productName)
		{
			return new EntityAlreadyExistsException("Release",
				$"A release already named '{releaseName}' already exists for product '{productName}'."
			);
		}
	}
}