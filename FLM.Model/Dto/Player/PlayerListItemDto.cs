using System;

namespace FLM.Model.Dto.Player
{
	public class PlayerListItemDto
	{
		public int? Id { get; set; }
		public string FullName { get; set; }
		public DateTime DateOfBirth { get; set; }

		public int? TeamId { get; set; }
		public string TeamFullName { get; set; }
	}
}