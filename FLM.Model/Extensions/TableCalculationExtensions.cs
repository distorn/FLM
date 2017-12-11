using FLM.Model.Entities;
using FLM.Model.Exceptions;

namespace FLM.Model.Extensions
{
	public static class TableCalculationExtensions
	{
		public static void ApplyMatch(this TeamTableStanding tableStanding, Match match)
		{
			if (match.Team1Score == null || match.Team2Score == null)
			{
				throw new FlmModelException(
					"Can't apply match to team table standing. Match result wasn't provided."
				);
			}

			if (!(match.Team1Id == tableStanding.TeamId || match.Team2Id == tableStanding.TeamId))
			{
				throw new FlmModelException(
					"Can't apply match to team table standing. Team is not presented in provided match."
				);
			}

			var isHomeMatch = match.Team1Id == tableStanding.TeamId;

			// - Add Points -

			if (match.IsDraw())
			{
				// - Draw -
				tableStanding.Points += FootballConstants.PointsForDraw;
				tableStanding.MatchesDrawn++;
			}
			else
			{
				var isHomeWon = match.IsHomeTeamWon();
				if ((isHomeMatch && isHomeWon) || (!isHomeMatch && !isHomeWon))
				{
					// - Win -
					tableStanding.Points += FootballConstants.PointsForWin;
					tableStanding.MatchesWon++;
				}
				else
				{
					// - Lose -
					tableStanding.Points += FootballConstants.PointsForLose;
					tableStanding.MatchesLost++;
				}
			}

			tableStanding.MatchesPlayed++;

			// - Add Goals For/Against -

			tableStanding.GoalsFor += isHomeMatch ? match.Team1Score.Value : match.Team2Score.Value;
			tableStanding.GoalsAgainst += isHomeMatch ? match.Team2Score.Value : match.Team1Score.Value;
		}

		public static bool IsDraw(this Match match)
		{
			if (match.Team1Score.HasValue && match.Team2Score.HasValue)
			{
				return match.Team1Score == match.Team2Score;
			}
			else
			{
				return false;
			}
		}

		public static bool IsHomeTeamWon(this Match match)
		{
			if (match.Team1Score.HasValue && match.Team2Score.HasValue)
			{
				return match.Team1Score > match.Team2Score;
			}
			else
			{
				return false;
			}
		}

		public static bool IsHomeTeamLose(this Match match)
		{
			if (match.Team1Score.HasValue && match.Team2Score.HasValue)
			{
				return match.Team1Score < match.Team2Score;
			}
			else
			{
				return false;
			}
		}
	}
}