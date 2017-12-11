using System;

namespace FLM.Model.Dto.Team
{
	public class TeamPlayerDto
	{
		public int PlayerId { get; set; }
		public byte Number { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
	}
}