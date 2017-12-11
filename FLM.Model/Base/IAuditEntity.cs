using System;

namespace FLM.Model.Base
{
	public interface IAuditEntity : IEntity
	{
		string CreationUser { get; set; }

		DateTime? CreationDateTime { get; set; }

		string LastUpdateUser { get; set; }

		DateTime? LastUpdateDateTime { get; set; }
	}
}