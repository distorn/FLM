using FLM.BL.Responses;
using FLM.Model.Dto.League;
using FLM.Model.Dto.Team;
using FLM.Model.Entities;
using System.Threading.Tasks;

namespace FLM.BL.Contracts
{
	public interface ITeamService : IBusinessLogicService
	{
		Task<IPagingModelResponse<TeamListItemDto>> GetTeamsAsync(int pageSize, int pageNumber);

		Task<ISingleModelResponse<TeamDto>> GetTeamAsync(int id);

		Task<ISingleModelResponse<Team>> CreateTeamAsync(Team item);

		Task<ISingleModelResponse<Team>> UpdateTeamAsync(Team item);

		Task<ISingleModelResponse<Team>> RemoveTeamAsync(int id);

		Task<IListModelResponse<TeamLookupDto>> LookupTeamAsync(string searchCriteria);

		Task<IListModelResponse<TeamPlayerDto>> GetTeamPlayersAsync(int teamId);

		Task<IResponse> AssignPlayerToTeamAsync(int teamId, int playerId, byte number);

		Task<IResponse> RemovePlayerFromTeamAsync(int teamId, int playerId);

		Task<IListModelResponse<LeagueDto>> GetTeamLeaguesAsync(int teamId);
	}
}