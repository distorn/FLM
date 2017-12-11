using FLM.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLM.DAL.Contracts.Repositories
{
	public interface ILeagueRepository : IRepository<League>
	{
		IQueryable<TeamLeagueAssignment> GetTeamLeagueAssignments();

		Task<int> AddTeamAssignmentAsync(TeamLeagueAssignment item);

		Task<int> RemoveTeamAssignmentAsync(TeamLeagueAssignment item);

		IQueryable<TeamTableStanding> GetStandings();

		Task<int> AddTeamStandingsAsync(IEnumerable<TeamTableStanding> items);

		Task<int> RemoveTeamStandingsAsync(IEnumerable<TeamTableStanding> items);
	}
}