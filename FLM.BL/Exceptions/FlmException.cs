using System;

namespace FLM.BL.Exceptions
{
	public class FlmException : Exception
	{
		public FlmException()
			: base()
		{
		}

		public FlmException(String message)
			: base(message)
		{
		}

		public FlmException(String message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}