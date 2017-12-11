using FLM.Model.Base;
using System;
using System.Collections.Generic;

namespace FLM.Model.Entities
{
	public class League : BaseAuditEntity
	{
		public string Name { get; set; }
		public string Season { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public byte RoundsCount { get; set; }

		public List<TeamLeagueAssignment> Teams { get; set; }
		public List<TeamTableStanding> Standings { get; set; }
		public List<Match> Matches { get; set; }
	}
}