using AutoMapper.QueryableExtensions;
using FLM.BL.Contracts;
using FLM.BL.Exceptions;
using FLM.BL.Responses;
using FLM.DAL.Extensions;
using FLM.Model.Dto.League;
using FLM.Model.Dto.Match;
using FLM.Model.Entities;
using FLM.Model.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLM.BL.Services
{
	public class LeagueService : BusinessLogicService, ILeagueService
	{
		public LeagueService(IServiceProvider serviceProvider, ILogger<LeagueService> logger = null) : base(serviceProvider, logger)
		{
		}

		public async Task<IPagingModelResponse<LeagueListItemDto>> GetLeaguesAsync(int pageSize = 0, int pageNumber = 0)
		{
			Logger?.LogInformation($"{nameof(GetLeaguesAsync)} has been invoked");

			var response = new PagingModelResponse<LeagueListItemDto>();

			try
			{
				response.PageSize = pageSize;
				response.PageNumber = pageNumber;

				var query = LeagueRepository.GetItems()
					.OrderByDescending(entity => entity.CreationDateTime)
					.AsNoTracking();

				response.ItemCount = await query.CountAsync();
				query = query.Paging(pageSize, pageNumber);

				response.Model = await query.ProjectTo<LeagueListItemDto>().ToListAsync();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<LeagueDto>> GetLeagueAsync(int id)
		{
			Logger?.LogInformation($"{nameof(GetLeagueAsync)} has been invoked");

			var response = new SingleModelResponse<LeagueDto>();

			try
			{
				var item = await LeagueRepository.GetItemByIdAsync(id);

				if (ShouldIncludeAuditData())
				{
					response.Model = Mapper.Map<LeagueAuditDto>(item);
				}
				else
				{
					response.Model = Mapper.Map<LeagueDto>(item);
				}
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<League>> CreateLeagueAsync(League item)
		{
			Logger?.LogInformation($"{nameof(CreateLeagueAsync)} has been invoked");

			var response = new SingleModelResponse<League>();

			try
			{
				await LeagueRepository.AddItemAsync(item);
				response.Model = item;
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<League>> UpdateLeagueAsync(League item)
		{
			Logger?.LogInformation($"{nameof(UpdateLeagueAsync)} has been invoked");

			var response = new SingleModelResponse<League>();

			try
			{
				await LeagueRepository.UpdateItemAsync(item);
				response.Model = item;
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<League>> RemoveLeagueAsync(int id)
		{
			Logger?.LogInformation($"{nameof(RemoveLeagueAsync)} has been invoked");

			var response = new SingleModelResponse<League>();

			try
			{
				response.Model = await LeagueRepository.GetItemByIdAsync(id);

				if (response.Model != null)
				{
					await LeagueRepository.RemoveItemAsync(response.Model);
				}
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IResponse> AssignTeamToLeagueAsync(int leagueId, int teamId)
		{
			Logger?.LogInformation($"{nameof(AssignTeamToLeagueAsync)} has been invoked");

			var response = new SimpleResponse();

			try
			{
				var league = await GetAndValidateItem(leagueId);

				var team = await TeamRepository.GetItemByIdAsync(teamId);
				if (team == null)
				{
					throw new FlmException($"Team with id={teamId} doesn't exist");
				}

				var existingAssignment = await LeagueRepository.GetTeamLeagueAssignments()
						.FirstOrDefaultAsync(tla => tla.TeamId == teamId && tla.LeagueId == leagueId);

				if (existingAssignment != null)
				{
					throw new FlmException("Team is already assigned to this league");
				}

				await LeagueRepository.AddTeamAssignmentAsync(new TeamLeagueAssignment()
				{
					LeagueId = leagueId,
					TeamId = teamId
				});

				// TODO: As we really do not need a result of this task call it in background. Consider using Hangfire.
				await RecalculateLeagueStandingsAsync(leagueId);
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IResponse> RemoveTeamFromLeagueAsync(int leagueId, int teamId)
		{
			Logger?.LogInformation($"{nameof(RemoveTeamFromLeagueAsync)} has been invoked");

			var response = new SimpleResponse();

			try
			{
				var league = await GetAndValidateItem(leagueId);

				var team = await TeamRepository.GetItemByIdAsync(teamId);
				if (team == null)
				{
					throw new FlmException($"Team with id={teamId} doesn't exist");
				}

				var assignment = await LeagueRepository.GetTeamLeagueAssignments()
					.FirstOrDefaultAsync(tla => tla.TeamId == teamId && tla.LeagueId == leagueId);

				if (assignment == null)
				{
					throw new FlmException($"Team with id={teamId} isn't assigned to the league with id={leagueId}");
				}

				await LeagueRepository.RemoveTeamAssignmentAsync(assignment);

				// Remove all team Matches in this league
				var teamMatches = MatchRepository.GetItems()
					.Where(m => m.LeagueId == leagueId && (m.Team1Id == teamId || m.Team2Id == teamId));

				await MatchRepository.RemoveItemsAsync(teamMatches);

				// TODO: As we really do not need a result of this task call it in background. Consider using Hangfire.
				await RecalculateLeagueStandingsAsync(leagueId);
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IListModelResponse<LeagueTeamDto>> GetLeagueTeamsAsync(int leagueId)
		{
			Logger?.LogInformation($"{nameof(GetLeagueTeamsAsync)} has been invoked");

			var response = new ListModelResponse<LeagueTeamDto>();

			try
			{
				var league = await GetAndValidateItem(leagueId);

				var query = LeagueRepository.GetTeamLeagueAssignments()
						.Where(tla => tla.LeagueId == leagueId);

				response.Model = query.ProjectTo<LeagueTeamDto>().ToList();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IListModelResponse<MatchListItemDto>> GetRoundScheduleAsync(int leagueId, byte roundNum)
		{
			Logger?.LogInformation($"{nameof(GetRoundScheduleAsync)} has been invoked");

			var response = new ListModelResponse<MatchListItemDto>();

			try
			{
				var league = await GetAndValidateItem(leagueId, roundNum);

				var matches = MatchRepository.GetItems()
					.Where(m => m.LeagueId == leagueId && m.Round == roundNum);

				response.Model = matches.ProjectTo<MatchListItemDto>().ToList();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IListModelResponse<MatchListItemDto>> EditRoundScheduleAsync(int leagueId, byte roundNum, IEnumerable<Match> matches)
		{
			Logger?.LogInformation($"{nameof(EditRoundScheduleAsync)} has been invoked");

			var response = new ListModelResponse<MatchListItemDto>();

			try
			{
				var league = await GetAndValidateItem(leagueId, roundNum);

				var oldScheduleMatches = MatchRepository.GetItems().Where(m => m.LeagueId == leagueId && m.Round == roundNum);
				await MatchRepository.RemoveItemsAsync(oldScheduleMatches);
				await MatchRepository.AddItemsAsync(matches);

				response.Model = matches.Select(item => Mapper.Map<MatchListItemDto>(item)).ToList();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IResponse> ClearRoundScheduleAsync(int leagueId, byte roundNum)
		{
			Logger?.LogInformation($"{nameof(EditRoundScheduleAsync)} has been invoked");

			var response = new SimpleResponse();

			try
			{
				var league = await GetAndValidateItem(leagueId, roundNum);

				var oldScheduleMatches = MatchRepository.GetItems().Where(m => m.LeagueId == leagueId && m.Round == roundNum);
				await MatchRepository.RemoveItemsAsync(oldScheduleMatches);
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		private async Task<League> GetAndValidateItem(int leagueId, byte? roundNum = null)
		{
			var league = await LeagueRepository.GetItemByIdAsync(leagueId);

			if (league == null)
			{
				throw new FlmException($"League with id={leagueId} doesn't exist");
			}

			if (roundNum.HasValue && league.RoundsCount < roundNum)
			{
				throw new FlmException($"League {league.GetDisplayName()} has only {league.RoundsCount} rounds");
			}

			return league;
		}

		public async Task<IListModelResponse<TeamTableStandingDto>> GetStandingsAsync(int leagueId)
		{
			Logger?.LogInformation($"{nameof(GetStandingsAsync)} has been invoked");

			var response = new ListModelResponse<TeamTableStandingDto>();

			try
			{
				var league = await GetAndValidateItem(leagueId);

				var query = LeagueRepository.GetStandings()
						.Where(tts => tts.LeagueId == leagueId)
						.OrderBy(tts => tts.Position);

				response.Model = await query.ProjectTo<TeamTableStandingDto>().ToListAsync();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task RecalculateLeagueStandingsAsync(int leagueId)
		{
			var league = await GetAndValidateItem(leagueId);

			var teams = LeagueRepository.GetTeamLeagueAssignments()
						.Where(tla => tla.LeagueId == leagueId)
						.Select(tla => tla.TeamId);

			var currentStandings = new List<TeamTableStanding>();

			foreach (var teamId in teams)
			{
				var playedMatches = MatchRepository.GetItems()
					.Where(m => m.LeagueId == league.Id
						&& (m.Team1Id == teamId || m.Team2Id == teamId)
						&& m.Date < DateTime.Now);

				var tableStanding = new TeamTableStanding()
				{
					LeagueId = leagueId,
					TeamId = teamId
				};

				foreach (var match in playedMatches)
				{
					tableStanding.ApplyMatch(match);
				}

				currentStandings.Add(tableStanding);
			}

			LeagueTableSorter.CalculateTeamPositions(currentStandings);

			var oldStandings = LeagueRepository.GetStandings()
						.Where(tts => tts.LeagueId == leagueId);

			await LeagueRepository.RemoveTeamStandingsAsync(oldStandings);
			await LeagueRepository.AddTeamStandingsAsync(currentStandings);
		}
	}
}