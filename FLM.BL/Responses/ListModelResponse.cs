using System.Collections.Generic;

namespace FLM.BL.Responses
{
	public class ListModelResponse<TModel> : SimpleResponse, IListModelResponse<TModel>
	{
		public IEnumerable<TModel> Model { get; set; }
	}
}