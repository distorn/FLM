namespace FLM.BL.Responses
{
	public class PagingModelResponse<TModel> : ListModelResponse<TModel>, IPagingModelResponse<TModel>
	{
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
		public int ItemCount { get; set; }

		public int PageCount
		{
			get
			{
				if (ItemCount == 0)
				{
					return 0;
				}
				else if (PageSize == 0)
				{
					return 1;
				}
				return ItemCount / PageSize;
			}
		}
	}
}