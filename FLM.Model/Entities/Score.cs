using FLM.Model.Base;

namespace FLM.Model.Entities
{
	public class Score : BaseEntity
	{
		public int? MatchId { get; set; }
		public Match Match { get; set; }

		public int? TeamId { get; set; }
		public Team Team { get; set; }

		public int? EnemyTeamId { get; set; }
		public Team EnemyTeam { get; set; }

		public int? PlayerId { get; set; }
		public Player Player { get; set; }

		public byte Minute { get; set; }

		public bool IsOG { get; set; } // is Own Goal
		public bool IsPenalty { get; set; }
	}
}