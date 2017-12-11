using System;

namespace FLM.Model.Dto.Player
{
	public class PlayerLookupDto
	{
		public int? Id { get; set; }
		public string FullName { get; set; }
		public DateTime DateOfBirth { get; set; }
	}
}