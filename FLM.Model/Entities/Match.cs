using FLM.Model.Base;
using System;
using System.Collections.Generic;

namespace FLM.Model.Entities
{
	public class Match : BaseAuditEntity
	{
		public DateTime Date { get; set; }

		public int? LeagueId { get; set; }
		public League League { get; set; }

		public byte Round { get; set; }

		public int? Team1Id { get; set; }
		public Team Team1 { get; set; }
		public byte? Team1Score { get; set; }

		public int? Team2Id { get; set; }
		public Team Team2 { get; set; }
		public byte? Team2Score { get; set; }

		public List<Score> Scores { get; set; }
	}
}