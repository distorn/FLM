using FLM.DAL.Contracts;
using FLM.DAL.Contracts.Repositories;
using FLM.DAL.EFCore.Repositories.Base;
using FLM.Model.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FLM.DAL.EFCore.Repositories
{
	public class TeamRepository : BaseEFRepository<Team>, ITeamRepository
	{
		public TeamRepository(FootballDbContext context, IUserResolver userResolver) : base(context, userResolver)
		{
		}

		public IQueryable<PlayerTeamAssignment> GetPlayerTeamAssignments()
		{
			return Context.PlayerTeamAssignments;
		}

		public async Task<int> AddPlayerAssignmentAsync(PlayerTeamAssignment item)
		{
			await Context.PlayerTeamAssignments.AddAsync(item);
			return await CommitChangesAsync();
		}

		public async Task<int> RemovePlayerAssignmentAsync(PlayerTeamAssignment item)
		{
			Context.PlayerTeamAssignments.Remove(item);
			return await CommitChangesAsync();
		}
	}
}