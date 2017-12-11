using System;

namespace FLM.Model.Base
{
	public class BaseAuditEntity : BaseEntity, IAuditEntity
	{
		public string CreationUser { get; set; }
		public DateTime? CreationDateTime { get; set; }
		public string LastUpdateUser { get; set; }
		public DateTime? LastUpdateDateTime { get; set; }
	}
}