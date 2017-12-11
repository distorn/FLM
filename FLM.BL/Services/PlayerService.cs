using AutoMapper.QueryableExtensions;
using FLM.BL.Contracts;
using FLM.BL.Extensions;
using FLM.BL.Responses;
using FLM.DAL.Extensions;
using FLM.Model.Dto.Player;
using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FLM.BL.Services
{
	public class PlayerService : BusinessLogicService, IPlayerService
	{
		public const int LookupMaxItems = 10;

		public PlayerService(IServiceProvider serviceProvider, ILogger<PlayerService> logger = null) : base(serviceProvider, logger)
		{
		}

		public async Task<IPagingModelResponse<PlayerListItemDto>> GetPlayersAsync(int pageSize = 0, int pageNumber = 0)
		{
			Logger?.LogInformation($"{nameof(GetPlayersAsync)} has been invoked");

			var response = new PagingModelResponse<PlayerListItemDto>();

			try
			{
				response.PageSize = pageSize;
				response.PageNumber = pageNumber;

				var query = PlayerRepository.GetItems()
					.OrderByDescending(entity => entity.CreationDateTime)
					.AsNoTracking();

				response.ItemCount = await query.CountAsync();
				query = query.Paging(pageSize, pageNumber);

				response.Model = await query.ProjectTo<PlayerListItemDto>().ToListAsync();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<PlayerDto>> GetPlayerAsync(int id)
		{
			Logger?.LogInformation($"{nameof(GetPlayerAsync)} has been invoked");

			var response = new SingleModelResponse<PlayerDto>();

			try
			{
				var item = await PlayerRepository.GetItems()
					.Include(p => p.TeamAssignment)
						.ThenInclude(pta => pta.Team)
					.FirstOrDefaultAsync(p => p.Id == id);

				if (ShouldIncludeAuditData())
				{
					response.Model = Mapper.Map<PlayerAuditDto>(item);
				}
				else
				{
					response.Model = Mapper.Map<PlayerDto>(item);
				}
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<Player>> CreatePlayerAsync(Player item)
		{
			Logger?.LogInformation($"{nameof(CreatePlayerAsync)} has been invoked");

			var response = new SingleModelResponse<Player>();

			try
			{
				await PlayerRepository.AddItemAsync(item);
				response.Model = item;
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<Player>> UpdatePlayerAsync(Player item)
		{
			Logger?.LogInformation($"{nameof(UpdatePlayerAsync)} has been invoked");

			var response = new SingleModelResponse<Player>();

			try
			{
				await PlayerRepository.UpdateItemAsync(item);
				response.Model = item;
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<ISingleModelResponse<Player>> RemovePlayerAsync(int id)
		{
			Logger?.LogInformation($"{nameof(RemovePlayerAsync)} has been invoked");

			var response = new SingleModelResponse<Player>();

			try
			{
				response.Model = await PlayerRepository.GetItemByIdAsync(id);
				if (response.Model != null)
				{
					await PlayerRepository.RemoveItemAsync(response.Model);
				}
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}

		public async Task<IListModelResponse<PlayerLookupDto>> LookupPlayerAsync(string searchCriteria)
		{
			Logger?.LogInformation($"{nameof(LookupPlayerAsync)} [{searchCriteria}] has been invoked");

			var response = new ListModelResponse<PlayerLookupDto>();

			try
			{
				var keywords = searchCriteria.ToLower().GetWords();

				var query = PlayerRepository.GetItems().Where(
					player => keywords.All(keyword =>
						player.FirstName.ToLower().Contains(keyword) ||
						player.LastName.ToLower().Contains(keyword)
					)
				);

				query = query.OrderBy(entity => entity.CreationDateTime).AsNoTracking();

				query = query.Paging(LookupMaxItems, 1);
				var items = query.Select(item => Mapper.Map<PlayerLookupDto>(item));
				response.Model = await items.ToListAsync();
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}
	}
}