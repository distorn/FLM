using FLM.DAL.Contracts;
using FLM.DAL.Contracts.Repositories;
using FLM.DAL.EFCore.Repositories.Base;
using FLM.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLM.DAL.EFCore.Repositories
{
	public class LeagueRepository : BaseEFRepository<League>, ILeagueRepository
	{
		public LeagueRepository(FootballDbContext context, IUserResolver userResolver) : base(context, userResolver)
		{
		}

		public IQueryable<TeamLeagueAssignment> GetTeamLeagueAssignments()
		{
			return Context.TeamLeagueAssignments;
		}

		public async Task<int> AddTeamAssignmentAsync(TeamLeagueAssignment item)
		{
			await Context.TeamLeagueAssignments.AddAsync(item);
			return await CommitChangesAsync();
		}

		public async Task<int> RemoveTeamAssignmentAsync(TeamLeagueAssignment item)
		{
			Context.TeamLeagueAssignments.Remove(item);
			return await CommitChangesAsync();
		}

		public IQueryable<TeamTableStanding> GetStandings()
		{
			return Context.TableStandings;
		}

		public async Task<int> AddTeamStandingsAsync(IEnumerable<TeamTableStanding> items)
		{
			await Context.TableStandings.AddRangeAsync(items);
			return await CommitChangesAsync();
		}

		public async Task<int> RemoveTeamStandingsAsync(IEnumerable<TeamTableStanding> items)
		{
			Context.TableStandings.RemoveRange(items);
			return await CommitChangesAsync();
		}
	}
}