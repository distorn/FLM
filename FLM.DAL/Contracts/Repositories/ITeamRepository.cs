using FLM.Model.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FLM.DAL.Contracts.Repositories
{
	public interface ITeamRepository : IRepository<Team>
	{
		IQueryable<PlayerTeamAssignment> GetPlayerTeamAssignments();

		Task<int> AddPlayerAssignmentAsync(PlayerTeamAssignment item);

		Task<int> RemovePlayerAssignmentAsync(PlayerTeamAssignment item);
	}
}