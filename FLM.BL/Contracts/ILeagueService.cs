using FLM.BL.Responses;
using FLM.Model.Dto.League;
using FLM.Model.Dto.Match;
using FLM.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLM.BL.Contracts
{
	public interface ILeagueService : IBusinessLogicService
	{
		Task<IPagingModelResponse<LeagueListItemDto>> GetLeaguesAsync(int pageSize, int pageNumber);

		Task<ISingleModelResponse<LeagueDto>> GetLeagueAsync(int id);

		Task<ISingleModelResponse<League>> CreateLeagueAsync(League item);

		Task<ISingleModelResponse<League>> UpdateLeagueAsync(League item);

		Task<ISingleModelResponse<League>> RemoveLeagueAsync(int id);

		Task<IListModelResponse<LeagueTeamDto>> GetLeagueTeamsAsync(int leagueId);

		Task<IResponse> AssignTeamToLeagueAsync(int leagueId, int teamId);

		Task<IResponse> RemoveTeamFromLeagueAsync(int leagueId, int teamId);

		Task<IListModelResponse<MatchListItemDto>> GetRoundScheduleAsync(int leagueId, byte roundNum);

		Task<IListModelResponse<MatchListItemDto>> EditRoundScheduleAsync(int leagueId, byte roundNum, IEnumerable<Match> matches);

		Task<IResponse> ClearRoundScheduleAsync(int leagueId, byte roundNum);

		Task<IListModelResponse<TeamTableStandingDto>> GetStandingsAsync(int leagueId);

		Task RecalculateLeagueStandingsAsync(int leagueId);
	}
}