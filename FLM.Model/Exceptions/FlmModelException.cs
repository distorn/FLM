using System;

namespace FLM.Model.Exceptions
{
	public class FlmModelException : Exception
	{
		public FlmModelException()
			: base()
		{
		}

		public FlmModelException(String message)
			: base(message)
		{
		}

		public FlmModelException(String message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}