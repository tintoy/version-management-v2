using System;

namespace DD.Cloud.VersionManagement
{
    /// <summary>
	///     The base class for exceptions raised by the version management application.
	/// </summary>
	public class VersionManagementException
		: Exception
	{
        /// <summary>
		///     Create a new <see cref="VersionManagementException"/>.
		/// </summary>
		/// <param name="messageOrFormat">
		///     The exception message or message-format specifier.
        /// </param>
		/// <param name="formatArguments">
		///     Optional format arguments.
        /// </param>
		public VersionManagementException(string messageOrFormat, params object[] formatArguments)
			: base(String.Format(messageOrFormat, formatArguments))
		{
		}

        /// <summary>
		///     Create a new <see cref="VersionManagementException"/>.
		/// </summary>
		/// <param name="innerException">
		///     The exception that caused the current exception to be raised.
        /// </param>
		/// <param name="messageOrFormat">
		///     The exception message or message-format specifier.
        /// </param>
		/// <param name="formatArguments">
		///     Optional format arguments.
        /// </param>
		public VersionManagementException(Exception innerException, string messageOrFormat, params object[] formatArguments)
			: base(String.Format(messageOrFormat, formatArguments), innerException)
		{
		}
	}
}