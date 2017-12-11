using FLM.Model.Base;

namespace FLM.Model.Entities
{
	public class TeamLeagueAssignment : BaseEntity
	{
		public int? LeagueId { get; set; }
		public League League { get; set; }

		public int? TeamId { get; set; }
		public Team Team { get; set; }
	}
}