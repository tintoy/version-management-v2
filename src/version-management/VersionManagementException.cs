using System;

namespace DD.Cloud.VersionManagement
{
	public class VersionManagementException
		: Exception
	{
		public VersionManagementException(string messageOrFormat, params object[] formatArguments)
			: base(String.Format(messageOrFormat, formatArguments))
		{
		}

		public VersionManagementException(Exception innerException, string messageOrFormat, params object[] formatArguments)
			: base(String.Format(messageOrFormat, formatArguments), innerException)
		{
		}
	}
}