using System;

namespace FLM.Model.Dto.Match
{
	public class MatchListItemDto
	{
		public int? Id { get; set; }
		public DateTime Date { get; set; }

		public int? Team1Id { get; set; }
		public string Team1FullName { get; set; }
		public byte? Team1Score { get; set; }

		public int? Team2Id { get; set; }
		public string Team2FullName { get; set; }
		public byte? Team2Score { get; set; }

		public int? LeagueId { get; set; }
		public string LeagueFullName { get; set; }

		public byte Round { get; set; }
	}
}