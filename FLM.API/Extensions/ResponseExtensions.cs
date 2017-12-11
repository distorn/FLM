using FLM.BL.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FLM.API.Extensions
{
	public static class ResponseExtensions
	{
		public static IActionResult ToHttpResponse<TModel>(this IListModelResponse<TModel> response)
		{
			var status = HttpStatusCode.OK;

			if (response.IsError)
			{
				status = HttpStatusCode.InternalServerError;
			}
			else if (response.Model == null)
			{
				status = HttpStatusCode.NoContent;
			}

			return new ObjectResult(response)
			{
				StatusCode = (int)status
			};
		}

		public static IActionResult ToHttpResponse<TModel>(this ISingleModelResponse<TModel> response)
		{
			var status = HttpStatusCode.OK;

			if (response.IsError)
			{
				status = HttpStatusCode.InternalServerError;
			}
			else if (response.Model == null)
			{
				status = HttpStatusCode.NotFound;
			}

			return new ObjectResult(response)
			{
				StatusCode = (int)status
			};
		}

		public static IActionResult ToHttpResponse(this IResponse response)
		{
			var status = HttpStatusCode.OK;

			if (response.IsError)
			{
				status = HttpStatusCode.InternalServerError;
			}

			return new ObjectResult(response)
			{
				StatusCode = (int)status
			};
		}
	}
}