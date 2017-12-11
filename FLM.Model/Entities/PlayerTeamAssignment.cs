using FLM.Model.Base;

namespace FLM.Model.Entities
{
	public class PlayerTeamAssignment : BaseEntity
	{
		public int? PlayerId { get; set; }
		public Player Player { get; set; }

		public int? TeamId { get; set; }
		public Team Team { get; set; }

		public byte Number { get; set; }
	}
}