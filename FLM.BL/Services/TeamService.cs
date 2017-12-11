using AutoMapper.QueryableExtensions;
using FLM.BL.Contracts;
using FLM.BL.Exceptions;
using FLM.BL.Extensions;
using FLM.BL.Responses;
using FLM.DAL.Extensions;
using FLM.Model.Dto.League;
using FLM.Model.Dto.Team;
using FLM.Model.Entities;
using FLM.Model.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FLM.BL.Services
{
	public class TeamService : BusinessLogicService, ITeamService
	{
		public const int lookupMaxItems = 10;

		public TeamService(IServiceProvider serviceProvider, ILogger<TeamService> logger = null) : base(serviceProvider, logger)
		{
		}

		public async Task<IPagingModelResponse<TeamListItemDto>> GetTeamsAsync(int pageSize = 0, int pageNumber = 0)
		{
			Logger?.LogInformation($"{nameof(GetTeamsAsync)} has been invoked");

			var response = new PagingModelResponse<TeamListItemDto>();

			try
			{
				response.PageSize = pageSize;
				response.PageNumber = pageNumber;

				var query = TeamRepository.GetItems()
					.OrderByDescending(entity => entity.CreationDateTime)
					.AsNoTracking();

				response.ItemCount = await query.CountAsync();
				query = query.Paging(pageSize, pageNumber);
				response.Model = await query.ProjectTo<TeamListItemDto>().ToListAsync();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<TeamDto>> GetTeamAsync(int id)
		{
			Logger?.LogInformation($"{nameof(GetTeamAsync)} has been invoked");

			var response = new SingleModelResponse<TeamDto>();

			try
			{
				var item = await TeamRepository.GetItemByIdAsync(id);

				if (ShouldIncludeAuditData())
				{
					response.Model = Mapper.Map<TeamAuditDto>(item);
				}
				else
				{
					response.Model = Mapper.Map<TeamDto>(item);
				}
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<Team>> CreateTeamAsync(Team item)
		{
			Logger?.LogInformation($"{nameof(CreateTeamAsync)} has been invoked");

			var response = new SingleModelResponse<Team>();

			try
			{
				await TeamRepository.AddItemAsync(item);
				response.Model = item;
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<Team>> UpdateTeamAsync(Team item)
		{
			Logger?.LogInformation($"{nameof(UpdateTeamAsync)} has been invoked");

			var response = new SingleModelResponse<Team>();

			try
			{
				await TeamRepository.UpdateItemAsync(item);
				response.Model = item;
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<Team>> RemoveTeamAsync(int id)
		{
			Logger?.LogInformation($"{nameof(RemoveTeamAsync)} has been invoked");

			var response = new SingleModelResponse<Team>();

			try
			{
				response.Model = await TeamRepository.GetItemByIdAsync(id);
				if (response.Model != null)
				{
					await TeamRepository.RemoveItemAsync(response.Model);
				}
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IResponse> AssignPlayerToTeamAsync(int teamId, int playerId, byte number)
		{
			Logger?.LogInformation($"{nameof(AssignPlayerToTeamAsync)} has been invoked");

			var response = new SimpleResponse();

			try
			{
				var team = await TeamRepository.GetItemByIdAsync(teamId);
				if (team == null)
				{
					throw new FlmException($"Team with id={teamId} doesn't exist");
				}

				var player = await PlayerRepository.GetItemByIdAsync(playerId);
				if (player == null)
				{
					throw new FlmException($"Player with id={playerId} doesn't exist");
				}

				var existingAssignment = await TeamRepository.GetPlayerTeamAssignments()
						.Include(pta => pta.Team)
						.FirstOrDefaultAsync(pta => pta.PlayerId == playerId);

				if (existingAssignment != null)
				{
					var msg = $"{player.GetDisplayName()} (id={playerId}) is already assigned " +
						$"to team: { existingAssignment.Team.GetDisplayName()}";
					throw new FlmException(msg);
				}

				var numberAssignment = TeamRepository.GetPlayerTeamAssignments()
						.Include(pta => pta.Player)
						.FirstOrDefault(pta => pta.TeamId == teamId && pta.Number == number);

				if (numberAssignment != null)
				{
					throw new FlmException($"Number {number} is already taken by other player: {numberAssignment.Player.GetDisplayName()}");
				}

				await TeamRepository.AddPlayerAssignmentAsync(new PlayerTeamAssignment()
				{
					TeamId = teamId,
					PlayerId = playerId,
					Number = number
				});

				Logger?.LogInformation($"Add {player} to {team}");
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IResponse> RemovePlayerFromTeamAsync(int teamId, int playerId)
		{
			Logger?.LogInformation($"{nameof(RemovePlayerFromTeamAsync)} has been invoked");

			var response = new SimpleResponse();

			try
			{
				var team = await TeamRepository.GetItemByIdAsync(teamId);
				if (team == null)
				{
					throw new FlmException($"Team with id={teamId} doesn't exist");
				}

				var player = await PlayerRepository.GetItemByIdAsync(playerId);
				if (player == null)
				{
					throw new FlmException($"Player with id={playerId} doesn't exist");
				}

				var assignment = await TeamRepository.GetPlayerTeamAssignments()
					.FirstOrDefaultAsync(pta => pta.PlayerId == playerId && pta.TeamId == teamId);

				if (assignment == null)
				{
					throw new FlmException($"Player with id={playerId} isn't assigned to the team with id={teamId}");
				}

				await TeamRepository.RemovePlayerAssignmentAsync(assignment);
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IListModelResponse<TeamPlayerDto>> GetTeamPlayersAsync(int teamId)
		{
			Logger?.LogInformation($"{nameof(GetTeamPlayersAsync)} has been invoked");

			var response = new ListModelResponse<TeamPlayerDto>();

			try
			{
				var team = await TeamRepository.GetItemByIdAsync(teamId);
				if (team == null)
				{
					throw new FlmException($"Team with id={teamId} doesn't exist");
				}

				var query = TeamRepository.GetPlayerTeamAssignments()
						.Where(pta => pta.TeamId == teamId);

				response.Model = await query.ProjectTo<TeamPlayerDto>().ToListAsync();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IListModelResponse<TeamLookupDto>> LookupTeamAsync(string searchCriteria)
		{
			Logger?.LogInformation($"{nameof(LookupTeamAsync)} [{searchCriteria}] has been invoked");

			var response = new ListModelResponse<TeamLookupDto>();

			try
			{
				var keywords = searchCriteria.ToLower().GetWords();

				var query = TeamRepository.GetItems().Where(
					team => keywords.All(keyword =>
						team.Name.ToLower().Contains(keyword) ||
						team.City.ToLower().Contains(keyword)
					)
				);

				query = query.OrderBy(entity => entity.CreationDateTime).AsNoTracking();

				query = query.Paging(lookupMaxItems, 1);
				response.Model = await query.ProjectTo<TeamLookupDto>().ToListAsync();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IListModelResponse<LeagueDto>> GetTeamLeaguesAsync(int teamId)
		{
			Logger?.LogInformation($"{nameof(GetTeamLeaguesAsync)} has been invoked");

			var response = new ListModelResponse<LeagueDto>();

			try
			{
				var team = await TeamRepository.GetItemByIdAsync(teamId);
				if (team == null)
				{
					throw new FlmException($"Team with id={teamId} doesn't exist");
				}

				var query = LeagueRepository.GetTeamLeagueAssignments()
						.Where(assignment => assignment.TeamId == teamId)
						.Select(assignment => assignment.League);

				response.Model = await query.ProjectTo<LeagueDto>().ToListAsync();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}
	}
}