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
	public class PlayerController : Controller
	{
		protected ILogger Logger;
		protected IPlayerService PlayerService;

		public PlayerController(ILogger<PlayerController> logger, IPlayerService playerService)
		{
			Logger = logger;
			PlayerService = playerService;
		}

		protected override void Dispose(Boolean disposing)
		{
			PlayerService?.Dispose();

			base.Dispose(disposing);
		}

		[HttpGet]
		public async Task<IActionResult> ListAsync(int pageSize = 10, int pageNumber = 1)
		{
			var response = await PlayerService.GetPlayersAsync(pageSize, pageNumber);
			return response.ToHttpResponse();
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetItemByIdAsync(int id)
		{
			var response = await PlayerService.GetPlayerAsync(id);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody]Player item)
		{
			var response = await PlayerService.CreatePlayerAsync(item);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpPut]
		public async Task<IActionResult> UpdateAsync([FromBody]Player item)
		{
			var response = await PlayerService.UpdatePlayerAsync(item);
			return response.ToHttpResponse();
		}

		[Authorize(Policies.CanEditData)]
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteAsync(Int32 id)
		{
			var response = await PlayerService.RemovePlayerAsync(id);
			return response.ToHttpResponse();
		}

		[HttpGet("/api/[controller]/lookup/{searchCriteria}")]
		public async Task<IActionResult> LookupAsync(string searchCriteria)
		{
			var response = await PlayerService.LookupPlayerAsync(searchCriteria);
			return response.ToHttpResponse();
		}
	}
}