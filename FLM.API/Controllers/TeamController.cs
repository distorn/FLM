using FLM.API.Extensions;
using FLM.API.Security;
using FLM.BL.Contracts;
using FLM.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FLM.API.Controllers
{
	[Route("api/[controller]")]
	public class TeamController : Controller
	{
		protected ILogger Logger;
		protected ITeamService TeamService;

		public TeamController(ILogger<TeamController> logger, ITeamService teamService)
		{
			Logger = logger;
			TeamService = teamService;
		}

		protected override void Dispose(Boolean disposing)
		{
			TeamService?.Dispose();

			base.Dispose(disposing);
		}

		[HttpGet]
		public async Task<IActionResult> ListAsync(int pageSize = 10, int pageNumber = 1)
		{
			var response = await TeamService.GetTeamsAsync(pageSize, pageNumber);
			return response.ToHttpResponse();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetItemByIdAsync(int id)
		{
			var response = await TeamService.GetTeamAsync(id);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody]Team item)
		{
			var response = await TeamService.CreateTeamAsync(item);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpPut]
		public async Task<IActionResult> UpdateAsync([FromBody]Team item)
		{
			var response = await TeamService.UpdateTeamAsync(item);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(Int32 id)
		{
			var response = await TeamService.RemoveTeamAsync(id);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpPost("/api/[controller]/{teamId}/assign-player/{playerId}/{number}")]
		public async Task<IActionResult> AssignPlayerToTeamAsync(int teamId, int playerId, byte number)
		{
			var response = await TeamService.AssignPlayerToTeamAsync(teamId, playerId, number);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpDelete("/api/[controller]/{teamId}/remove-player/{playerId}")]
		public async Task<IActionResult> RemovePlayerFromTeamAsync(int teamId, int playerId)
		{
			var response = await TeamService.RemovePlayerFromTeamAsync(teamId, playerId);
			return response.ToHttpResponse();
		}

		[HttpGet("/api/[controller]/{teamId}/players")]
		public async Task<IActionResult> GetTeamPlayersAsync(int teamId)
		{
			var response = await TeamService.GetTeamPlayersAsync(teamId);
			return response.ToHttpResponse();
		}

		[HttpGet("/api/[controller]/{teamId}/leagues")]
		public async Task<IActionResult> GetTeamLeaguesAsync(int teamId)
		{
			var response = await TeamService.GetTeamLeaguesAsync(teamId);
			return response.ToHttpResponse();
		}

		[HttpGet("/api/[controller]/lookup/{searchCriteria}")]
		public async Task<IActionResult> LookupAsync(string searchCriteria)
		{
			var response = await TeamService.LookupTeamAsync(searchCriteria);
			return response.ToHttpResponse();
		}
	}
}