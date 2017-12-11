using FLM.API.Extensions;
using FLM.API.Security;
using FLM.API.ViewModels;
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
	public class LeagueController : Controller
	{
		protected ILogger Logger;
		protected ILeagueService LeagueService;

		public LeagueController(ILogger<LeagueController> logger, ILeagueService leagueService)
		{
			Logger = logger;
			LeagueService = leagueService;
		}

		protected override void Dispose(Boolean disposing)
		{
			LeagueService?.Dispose();

			base.Dispose(disposing);
		}

		[HttpGet]
		public async Task<IActionResult> ListAsync(int pageSize = 10, int pageNumber = 1)
		{
			var response = await LeagueService.GetLeaguesAsync(pageSize, pageNumber);
			return response.ToHttpResponse();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetItemByIdAsync(int id)
		{
			var response = await LeagueService.GetLeagueAsync(id);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody]League item)
		{
			var response = await LeagueService.CreateLeagueAsync(item);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpPut]
		public async Task<IActionResult> UpdateAsync([FromBody]League item)
		{
			var response = await LeagueService.UpdateLeagueAsync(item);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(Int32 id)
		{
			var response = await LeagueService.RemoveLeagueAsync(id);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpPost("/api/[controller]/{leagueId}/assign-team/{teamId}")]
		public async Task<IActionResult> AssignTeamToLeagueAsync(int leagueId, int teamId)
		{
			var response = await LeagueService.AssignTeamToLeagueAsync(leagueId, teamId);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpDelete("/api/[controller]/{leagueId}/remove-team/{teamId}")]
		public async Task<IActionResult> RemoveTeamFromLeagueAsync(int leagueId, int teamId)
		{
			var response = await LeagueService.RemoveTeamFromLeagueAsync(leagueId, teamId);
			return response.ToHttpResponse();
		}

		[HttpGet("/api/[controller]/{leagueId}/teams")]
		public async Task<IActionResult> GetLeagueTeamsAsync(int leagueId)
		{
			var response = await LeagueService.GetLeagueTeamsAsync(leagueId);
			return response.ToHttpResponse();
		}

		[HttpGet("/api/[controller]/{leagueId}/schedule/{roundNum}")]
		public async Task<IActionResult> GetRoundScheduleAsync(int leagueId, byte roundNum)
		{
			var response = await LeagueService.GetRoundScheduleAsync(leagueId, roundNum);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpPost("/api/[controller]/{leagueId}/schedule/{roundNum}")]
		public async Task<IActionResult> EditRoundScheduleAsync(int leagueId, byte roundNum, [FromBody] RoundScheduleViewModel schedule)
		{
			var response = await LeagueService.EditRoundScheduleAsync(leagueId, roundNum, schedule.Matches);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpDelete("/api/[controller]/{leagueId}/schedule/{roundNum}")]
		public async Task<IActionResult> ClearRoundScheduleAsync(int leagueId, byte roundNum)
		{
			var response = await LeagueService.ClearRoundScheduleAsync(leagueId, roundNum);
			return response.ToHttpResponse();
		}

		[HttpGet("/api/[controller]/{leagueId}/standings")]
		public async Task<IActionResult> GetLeagueStandingsAsync(int leagueId)
		{
			var response = await LeagueService.GetStandingsAsync(leagueId);
			return response.ToHttpResponse();
		}
	}
}