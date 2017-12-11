using System;

namespace FLM.Model.Dto.League
{
	public class LeagueListItemDto
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string Season { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public int TeamsCount { get; set; }
	}
}