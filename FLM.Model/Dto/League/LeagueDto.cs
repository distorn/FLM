using System;

namespace FLM.Model.Dto.League
{
	public class LeagueDto
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string Season { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public byte RoundsCount { get; set; }
	}
}