namespace FLM.BL.Responses
{
	public interface IPagingModelResponse<TModel> : IListModelResponse<TModel>
	{
		int ItemCount { get; set; }
		int PageCount { get; }
	}
}