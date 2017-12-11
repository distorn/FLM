using System.Collections.Generic;

namespace FLM.BL.Responses
{
	public interface IListModelResponse<TModel> : IResponse
	{
		IEnumerable<TModel> Model { get; set; }
	}
}