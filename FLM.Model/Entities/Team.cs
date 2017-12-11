using FLM.Model.Base;
using System.Collections.Generic;

namespace FLM.Model.Entities
{
	public class Team : BaseAuditEntity
	{
		public string Name { get; set; }
		public string City { get; set; }
		public int FoundationYear { get; set; }

		public List<PlayerTeamAssignment> Players { get; set; }
		public List<TeamLeagueAssignment> Leagues { get; set; }
		public List<TeamTableStanding> Standings { get; set; }

		public List<Match> HomeMatches { get; set; }
		public List<Match> AwayMatches { get; set; }

		public List<Score> ScoresFor { get; set; }
		public List<Score> ScoresAgainst { get; set; }
	}
}