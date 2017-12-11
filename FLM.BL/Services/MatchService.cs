using AutoMapper.QueryableExtensions;
using FLM.BL.Contracts;
using FLM.BL.Exceptions;
using FLM.BL.Responses;
using FLM.DAL.Extensions;
using FLM.Model.Dto.Match;
using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FLM.BL.Services
{
	public class MatchService : BusinessLogicService, IMatchService
	{
		private ILeagueService _leagueService;

		public MatchService(IServiceProvider serviceProvider, ILogger<MatchService> logger = null) : base(serviceProvider, logger)
		{
		}

		protected ILeagueService LeagueService
		{
			get { return _leagueService ?? (_leagueService = ServiceProvider.GetService<ILeagueService>()); }
		}

		public async Task<IPagingModelResponse<MatchListItemDto>> GetMatchesAsync(
			int pageSize = 0, int pageNumber = 0,
			int? leagueId = null, int? teamId = null,
			DateTime? startDate = null, DateTime? endDate = null)
		{
			Logger?.LogInformation($"{nameof(GetMatchesAsync)} has been invoked");

			var response = new PagingModelResponse<MatchListItemDto>();

			try
			{
				response.PageSize = pageSize;
				response.PageNumber = pageNumber;

				var query = MatchRepository.GetItems();

				// - Filters -

				if (leagueId.HasValue)
				{
					query = query.Where(match => match.LeagueId == leagueId);
				}

				if (teamId.HasValue)
				{
					query = query.Where(match => match.Team1Id == teamId || match.Team2Id == teamId);
				}

				if (startDate.HasValue)
				{
					query = query.Where(match => match.Date >= startDate);
				}

				if (endDate.HasValue)
				{
					query = query.Where(match => match.Date <= endDate);
				}

				// - Ordering -

				query = query.OrderBy(match => match.Date).AsNoTracking();

				// - Paging -

				response.ItemCount = await query.CountAsync();
				query = query.Paging(pageSize, pageNumber);

				response.Model = await query.ProjectTo<MatchListItemDto>().ToListAsync();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<MatchDto>> GetMatchAsync(int id)
		{
			Logger?.LogInformation($"{nameof(GetMatchAsync)} has been invoked");

			var response = new SingleModelResponse<MatchDto>();

			try
			{
				var item = await MatchRepository.GetItems()
					.Include(m => m.Team1)
					.Include(m => m.Team2)
					.Include(m => m.League)
					.Include(m => m.Scores).ThenInclude(s => s.Player)
					.FirstOrDefaultAsync(m => m.Id == id);

				if (ShouldIncludeAuditData())
				{
					response.Model = Mapper.Map<MatchAuditDto>(item);
				}
				else
				{
					response.Model = Mapper.Map<MatchDto>(item);
				}
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<MatchDto>> UpdateMatchResultsAsync(Match results)
		{
			Logger?.LogInformation($"{nameof(UpdateMatchResultsAsync)} has been invoked");

			var response = new SingleModelResponse<MatchDto>();

			try
			{
				var item = await MatchRepository.GetItems()
					.Include(m => m.Scores)
					.FirstOrDefaultAsync(m => m.Id == results.Id);

				if (item == null)
				{
					throw new FlmException($"Match with id={results.Id} doesn't exist");
				}

				var homeTeamScores = results.Scores.Where(s => s.TeamId == item.Team1Id).Count();
				var awayTeamScores = results.Scores.Where(s => s.TeamId == item.Team2Id).Count();

				if (results.Team1Score != homeTeamScores || results.Team2Score != awayTeamScores)
				{
					throw new FlmException($"Provided scores mismatch match results.");
				}

				// Update results

				item.Team1Score = results.Team1Score;
				item.Team2Score = results.Team2Score;

				// Remove existed score items that are not presented in updated results

				item.Scores.RemoveAll(oldScore => !results.Scores.Any(s => s.Id == oldScore.Id));

				// Add new scores

				var newScores = results.Scores.Where(s => s.Id == null);

				foreach (var score in newScores)
				{
					item.Scores.Add(score);
				}

				await MatchRepository.UpdateItemAsync(item);

				// TODO: As we really do not need a result of this task call it in background. Consider using Hangfire.
				await LeagueService.RecalculateLeagueStandingsAsync(item.LeagueId.Value);

				// Return updated item with scores

				return await GetMatchAsync(item.Id.Value);
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}
	}
}