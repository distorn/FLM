namespace FLM.Model.Dto.League
{
	public class TeamTableStandingDto
	{
		public int? TeamId { get; set; }
		public string TeamFullName { get; set; }

		public byte Position { get; set; }

		public byte MatchesPlayed { get; set; }
		public byte MatchesWon { get; set; }
		public byte MatchesDrawn { get; set; }
		public byte MatchesLost { get; set; }

		public int GoalsFor { get; set; }
		public int GoalsAgainst { get; set; }

		public int Points { get; set; }
	}
}