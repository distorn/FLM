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
	public class MatchController : Controller
	{
		protected ILogger Logger;
		protected IMatchService MatchService;

		public MatchController(ILogger<MatchController> logger, IMatchService matchService)
		{
			Logger = logger;
			MatchService = matchService;
		}

		protected override void Dispose(Boolean disposing)
		{
			MatchService?.Dispose();

			base.Dispose(disposing);
		}

		[HttpPost("/api/[controller]/search")]
		public async Task<IActionResult> ListAsync([FromBody]MatchesFilterViewModel filter = null, int pageSize = 10, int pageNumber = 1)
		{
			var response = await MatchService.GetMatchesAsync(
				pageSize, pageNumber,
				filter?.LeagueId, filter?.TeamId, filter?.MinDate, filter?.MaxDate
			);
			return response.ToHttpResponse();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetItemByIdAsync(int id)
		{
			var response = await MatchService.GetMatchAsync(id);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpPut]
		public async Task<IActionResult> UpdateAsync([FromBody]Match results)
		{
			var response = await MatchService.UpdateMatchResultsAsync(results);
			return response.ToHttpResponse();
		}
	}
}