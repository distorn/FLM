using FLM.Model.Base;
using System;

namespace FLM.Model.Dto.Team
{
	public class TeamAuditDto : TeamDto, IAuditEntity
	{
		public string CreationUser { get; set; }
		public DateTime? CreationDateTime { get; set; }
		public string LastUpdateUser { get; set; }
		public DateTime? LastUpdateDateTime { get; set; }
	}
}