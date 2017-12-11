using FLM.BL.Responses;
using FLM.Model.Dto.Player;
using FLM.Model.Entities;
using System.Threading.Tasks;

namespace FLM.BL.Contracts
{
	public interface IPlayerService : IBusinessLogicService
	{
		Task<IPagingModelResponse<PlayerListItemDto>> GetPlayersAsync(int pageSize, int pageNumber);

		Task<IListModelResponse<PlayerLookupDto>> LookupPlayerAsync(string searchCriteria);

		Task<ISingleModelResponse<PlayerDto>> GetPlayerAsync(int id);

		Task<ISingleModelResponse<Player>> CreatePlayerAsync(Player item);

		Task<ISingleModelResponse<Player>> UpdatePlayerAsync(Player item);

		Task<ISingleModelResponse<Player>> RemovePlayerAsync(int id);
	}
}