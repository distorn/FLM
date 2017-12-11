using FLM.BL.Responses;
using FLM.Model.Dto.Match;
using FLM.Model.Entities;
using System;
using System.Threading.Tasks;

namespace FLM.BL.Contracts
{
	public interface IMatchService : IBusinessLogicService
	{
		Task<IPagingModelResponse<MatchListItemDto>> GetMatchesAsync(int pageSize, int pageNumber, int? leagueId = null, int? teamId = null, DateTime? startDate = null, DateTime? endDate = null);

		Task<ISingleModelResponse<MatchDto>> GetMatchAsync(int id);

		Task<ISingleModelResponse<MatchDto>> UpdateMatchResultsAsync(Match results);
	}
}