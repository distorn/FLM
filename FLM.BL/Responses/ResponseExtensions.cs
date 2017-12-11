using FLM.BL.Exceptions;
using Microsoft.Extensions.Logging;
using System;

namespace FLM.BL.Responses
{
	public static class ResponseExtensions
	{
		public const string DefaultErrorMessage = "There was an internal error, please contact to technical support.";

		public static void SetError(this IResponse response, Exception ex, ILogger logger)
		{
			response.IsError = true;

			var cast = ex as FlmException;

			if (cast == null)
			{
				response.ErrorMessage = DefaultErrorMessage;
				logger?.LogCritical(ex.ToString());
			}
			else
			{
				response.ErrorMessage = ex.Message;
				logger?.LogError(ex.Message);
			}
		}
	}
}