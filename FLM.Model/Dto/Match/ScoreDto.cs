namespace FLM.Model.Dto.Match
{
	public class ScoreDto
	{
		public int? Id { get; set; }

		public int? MatchId { get; set; }
		public int? TeamId { get; set; }
		public int? EnemyTeamId { get; set; }

		public int? PlayerId { get; set; }
		public string PlayerFullName { get; set; }

		public byte Minute { get; set; }

		public bool isOG { get; set; }
		public bool isPenalty { get; set; }
	}
}