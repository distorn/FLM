namespace FLM.BL.Responses
{
	public class SingleModelResponse<TModel> : SimpleResponse, ISingleModelResponse<TModel> where TModel : new()
	{
		public TModel Model { get; set; }
	}
}