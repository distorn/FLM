using FLM.Model.Entities;
using System.Collections.Generic;

namespace FLM.Model.Extensions
{
	public static class LeagueTableSorter
	{
		public static void CalculateTeamPositions(List<TeamTableStanding> tableRows)
		{
			tableRows.Sort(TablePositionComparer);
			tableRows.Reverse();

			for (byte i = 0; i < tableRows.Count; i++)
			{
				tableRows[i].Position = (byte)(i + 1);
			}
		}

		private static int TablePositionComparer(TeamTableStanding team1, TeamTableStanding team2)
		{
			// compare by teams points

			if (team1.Points > team2.Points) return 1;
			if (team1.Points < team2.Points) return -1;

			// if teams have equal points, compare by goals difference

			var team1GD = team1.GoalsDifference;
			var team2GD = team2.GoalsDifference;

			if (team1GD > team2GD) return 1;
			if (team1GD < team2GD) return -1;

			// if teams have equal goals difference, compare by what team scores more

			if (team1.GoalsFor > team2.GoalsFor) return 1;
			if (team1.GoalsFor < team2.GoalsFor) return -1;

			// TODO: provide additional comparison criteria for the case team1.GoalsFor == team2.GoalsFor
			return 0;
		}
	}
}