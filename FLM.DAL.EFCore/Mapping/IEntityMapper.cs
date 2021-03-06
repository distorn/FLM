using Microsoft.EntityFrameworkCore;

namespace FLM.DAL.EFCore.Mapping
{
	public interface IEntityMapper
	{
		void MapEntities(ModelBuilder modelBuilder);
	}
}