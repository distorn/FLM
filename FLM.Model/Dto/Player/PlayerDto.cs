using System;

namespace FLM.Model.Dto.Player
{
	public class PlayerDto
	{
		public int? Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }

		public int? TeamId { get; set; }
		public string TeamFullName { get; set; }
	}
}