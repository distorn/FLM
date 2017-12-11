using FLM.Model.Base;
using System;
using System.Collections.Generic;

namespace FLM.Model.Entities
{
	public class Player : BaseAuditEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }

		public PlayerTeamAssignment TeamAssignment { get; set; }

		public List<Score> Scores { get; set; }
	}
}