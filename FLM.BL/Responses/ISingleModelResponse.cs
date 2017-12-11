namespace FLM.BL.Responses
{
	public interface ISingleModelResponse<TModel> : IResponse
	{
		TModel Model { get; set; }
	}
}