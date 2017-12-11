using FLM.DAL.Contracts;
using FLM.DAL.EFCore;
using FLM.DAL.Mocks.Factories;
using FLM.Model;
using FLM.Model.Entities;
using FLM.Model.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FLM.DAL.Mocks
{
	public class MockDbInitializer : IDbInitializer
	{
		public const byte LeaguesCount = 2;

		public const byte LeagueRoundRobinsCount = 2;
		public const byte TeamsPerLeague = 8;
		public const byte RoundsPerCircle = TeamsPerLeague - 1;
		public const byte MatchesPerRound = TeamsPerLeague / 2;
		public const byte RoundsCount = RoundsPerCircle * LeagueRoundRobinsCount;

		public const byte PlayersPerTeamMin = 20;
		public const byte PlayersPerTeamMax = 30;

		public const int TeamsCount = LeaguesCount * TeamsPerLeague;
		public const int PlayersCount = TeamsCount * PlayersPerTeamMax;

		public const byte MaxScoresPerTeamInMatch = 5;

		public const float PenaltyChance = 0.1f;
		public const float OwnGoalChance = 0.02f;

		public const byte DaysInWeek = 7;

		private readonly FootballDbContext _context;
		private readonly ILogger _logger;
		private readonly IServiceProvider _serviceProvider;

		public MockDbInitializer(
			FootballDbContext context,
			ILogger<MockDbInitializer> logger,
			IServiceProvider serviceProvider
			)
		{
			_context = context;
			_logger = logger;
			_serviceProvider = serviceProvider;
		}

		public void Initialize()
		{
			var logMessages = new List<string>();

			var totalTimer = Stopwatch.StartNew();
			var timer = Stopwatch.StartNew();

			//create database schema if none exists
			var recreated = _context.Database.EnsureCreated();

			if (recreated)
			{
				logMessages.Add($"Database schema created for {timer.ElapsedMilliseconds} ms.");
			}
			else
			{
				logMessages.Add($"Database schema was already created. Checked for {timer.ElapsedMilliseconds} ms.");
			}

			if (_context.Players.Any())
			{
				return; // DB has been already seeded
			}

			timer.Restart();

			AddPlayers();
			logMessages.Add($"{PlayersCount} players added for {timer.ElapsedMilliseconds} ms.");
			timer.Restart();

			AddTeams();
			logMessages.Add($"{TeamsCount} teams added for {timer.ElapsedMilliseconds} ms.");
			timer.Restart();

			AddLeagues();
			logMessages.Add($"{LeaguesCount} leagues added for {timer.ElapsedMilliseconds} ms.");
			timer.Restart();

			AssignPlayersToTeams(out int playersCount);
			logMessages.Add($"{TeamsCount} teams populated with {playersCount} players for {timer.ElapsedMilliseconds} ms.");
			timer.Restart();

			AssignTeamsToLeagues();
			logMessages.Add($"{TeamsCount} teams assigned to {LeaguesCount} leagues for {timer.ElapsedMilliseconds} ms.");
			timer.Restart();

			GenerateMatches(out int matchesCount);
			logMessages.Add($"{matchesCount} matches generated in {LeaguesCount} leagues for {timer.ElapsedMilliseconds} ms.");
			timer.Restart();

			GenerateMatchResults(out matchesCount, out int scoresCount);
			logMessages.Add($"{scoresCount} scores generated in {matchesCount} matches for {timer.ElapsedMilliseconds} ms.");
			timer.Restart();

			RecalculateLeagueTables();
			logMessages.Add($"League tables recalculated for {timer.ElapsedMilliseconds} ms.");
			timer.Stop();

			logMessages.Add($"Seeding Database completed for {totalTimer.ElapsedMilliseconds} ms.");
			totalTimer.Stop();

			if (_logger != null)
			{
				foreach (var message in logMessages)
				{
					_logger.LogInformation(message);
				}
			}
		}

		private void RecalculateLeagueTables()
		{
			var leagues = _context.Leagues.ToList();

			foreach (var league in leagues)
			{
				var teams = _context.TeamLeagueAssignments
					.Where(tla => tla.LeagueId == league.Id).Select(tla => tla.TeamId);

				var tableStandings = new List<TeamTableStanding>();

				foreach (var teamId in teams)
				{
					var playedMatches = _context.Matches
						.Where(m => m.LeagueId == league.Id
							&& (m.Team1Id == teamId || m.Team2Id == teamId)
							&& m.Date < DateTime.Now);

					var tableStanding = new TeamTableStanding()
					{
						LeagueId = league.Id,
						TeamId = teamId
					};

					foreach (var match in playedMatches)
					{
						tableStanding.ApplyMatch(match);
					}

					tableStandings.Add(tableStanding);
				}

				LeagueTableSorter.CalculateTeamPositions(tableStandings);
				_context.TableStandings.AddRange(tableStandings);
			}

			_context.SaveChanges();
		}

		private void GenerateMatchResults(out int matchesCount, out int scoresCount)
		{
			matchesCount = 0;
			scoresCount = 0;

			var matches = _context.Matches.Where(m => m.Date < DateTime.Now);
			var random = new Random();

			foreach (var match in matches)
			{
				match.Team1Score = (byte)random.Next(MaxScoresPerTeamInMatch);
				match.Team2Score = (byte)random.Next(MaxScoresPerTeamInMatch);

				var scoresInMatch = (byte)(match.Team1Score + match.Team2Score);

				if (scoresInMatch > 0)
				{
					var homeTeamPlayers = _context.PlayerTeamAssignments
						.Where(pta => pta.TeamId == match.Team1Id)
						.Select(pta => pta.PlayerId).ToList();

					var awayTeamPlayers = _context.PlayerTeamAssignments
						.Where(pta => pta.TeamId == match.Team2Id)
						.Select(pta => pta.PlayerId).ToList();

					// Home Team Scores
					for (var i = 0; i < match.Team1Score; i++)
					{
						var score = GenerateRandomScore(match, true);
						var candidates = score.IsOG ? awayTeamPlayers : homeTeamPlayers;
						score.PlayerId = candidates.ElementAt(random.Next(candidates.Count));
						_context.Scores.Add(score);
					}

					// Away Team Scores
					for (var i = 0; i < match.Team2Score; i++)
					{
						var score = GenerateRandomScore(match, false);
						var candidates = score.IsOG ? homeTeamPlayers : awayTeamPlayers;
						score.PlayerId = candidates.ElementAt(random.Next(candidates.Count));
						_context.Scores.Add(score);
					}
				}

				scoresCount += scoresInMatch;
				matchesCount++;
			}

			_context.SaveChanges();
		}

		private Score GenerateRandomScore(Match match, bool forHomeTeam)
		{
			var random = new Random();
			bool isOG = random.NextDouble() < OwnGoalChance;
			bool isPenalty = isOG ? false : random.NextDouble() < PenaltyChance;

			return new Score()
			{
				MatchId = match.Id,
				TeamId = forHomeTeam ? match.Team1Id : match.Team2Id,
				EnemyTeamId = forHomeTeam ? match.Team2Id : match.Team1Id,
				IsOG = isOG,
				IsPenalty = isPenalty,
				Minute = (byte)random.Next(1, FootballConstants.MinutesInMatch),
			};
		}

		private void GenerateMatches(out int matchesCount)
		{
			matchesCount = 0;

			var leagues = _context.Leagues.ToList();

			foreach (var league in leagues)
			{
				var leagueTeams = _context.TeamLeagueAssignments
					.Where(tla => tla.LeagueId == league.Id)
					.Select(tla => tla.Team);

				var circleSchedule = GetMatchesPerCircle(leagueTeams);

				// LEAGUE: CIRCLE
				for (var c = 0; c < LeagueRoundRobinsCount; c++)
				{
					var isEvenCircle = (c + 1) % 2 == 0;

					// LEAGUE: CIRCLE: ROUND
					for (var cr = 0; cr < RoundsPerCircle; cr++)
					{
						var roundNum = c * RoundsPerCircle + cr + 1;
						var roundDate = league.StartDate.AddDays((roundNum - 1) * DaysInWeek);

						// LEAGUE: CIRCLE: ROUND: MATCH
						for (var i = 0; i < MatchesPerRound; i++)
						{
							var index = (cr * MatchesPerRound) + i;
							var stub = circleSchedule.ElementAt(index);

							var match = new Match()
							{
								LeagueId = league.Id,
								Round = (byte)roundNum,
								Date = roundDate,
								Team1Id = isEvenCircle ? stub.Team1.Id : stub.Team2.Id,
								Team2Id = isEvenCircle ? stub.Team2.Id : stub.Team1.Id,
								CreationUser = Constants.AutoGenerated,
								CreationDateTime = DateTime.Now,
							};

							_context.Matches.Add(match);
							matchesCount++;
						};
					}
				}
			}
			_context.SaveChanges();
		}

		private List<MatchStub> GetMatchesPerCircle(IEnumerable<Team> teams)
		{
			var roster = teams.Select(item => item).ToList();
			var matches = new List<MatchStub>();

			// Solution based on Round-Robin Scheduling Algorithm
			// https://en.wikipedia.org/wiki/Round-robin_tournament#Scheduling_algorithm

			for (var rn = 0; rn < RoundsPerCircle; rn++)
			{
				for (var i = 0; i < MatchesPerRound; i++)
				{
					var teamA = roster.ElementAt(i);
					var teamB = roster.ElementAt(roster.Count - (i + 1));

					var match = new MatchStub() { Team1 = teamA, Team2 = teamB };

					if (i == 0 && rn % 2 == 0)
					{
						// fix home/away matches disballance for 1st team as it doesn't move its position in the list during the loop cycle
						match = new MatchStub() { Team1 = match.Team2, Team2 = match.Team1 };
					}

					matches.Add(match);
				}

				var shiftedTeam = roster.ElementAt(1);
				roster.Remove(shiftedTeam);
				roster.Add(shiftedTeam);
			}

			return matches;
		}

		private void AssignTeamsToLeagues()
		{
			var teamsIds = _context.Teams.Select(p => p.Id).ToArray();
			var leagueIds = _context.Leagues.Select(t => t.Id).ToArray();

			int teamIndex = 0;

			foreach (int leagueId in leagueIds)
			{
				for (byte i = 1; i <= TeamsPerLeague; i++)
				{
					_context.TeamLeagueAssignments.Add(new TeamLeagueAssignment()
					{
						LeagueId = leagueId,
						TeamId = teamsIds[teamIndex++],
					});
				}
			}
			_context.SaveChanges();
		}

		private void AssignPlayersToTeams(out int assignedPlayers)
		{
			assignedPlayers = 0;

			var playersIds = _context.Players.Select(p => p.Id).ToArray();
			var teamIds = _context.Teams.Select(t => t.Id).ToArray();

			var random = new Random();
			int playerIndex = 0;

			foreach (int teamId in teamIds)
			{
				var playersInTeam = random.Next(PlayersPerTeamMin, PlayersPerTeamMax);

				for (byte i = 1; i <= playersInTeam; i++)
				{
					_context.PlayerTeamAssignments.Add(new PlayerTeamAssignment()
					{
						TeamId = teamId,
						PlayerId = playersIds[playerIndex++],
						Number = i
					});
				}
			}

			assignedPlayers = playerIndex;
			_context.SaveChanges();
		}

		private void AddTeams()
		{
			var factory = new TeamMockFactory();
			for (int i = 0; i < TeamsCount; i++)
			{
				_context.Teams.Add(factory.CreateRandomTeam());
			}
			_context.SaveChanges();
		}

		private void AddLeagues()
		{
			for (int i = 0; i < LeaguesCount; i++)
			{
				var now = DateTime.Now;
				// Assuming that one round played in a league every week
				var duration = (RoundsCount - 1) * DaysInWeek;
				// 1st league should be already completed by now
				var startDate = now.AddDays(-duration);
				// every next generated league starts a month later than previous
				startDate = startDate.AddMonths(i);
				var endDate = startDate.AddDays(duration);
				var isSingleYear = startDate.Year == endDate.Year;

				_context.Leagues.Add(new League()
				{
					Name = $"Test League {i + 1}",
					Season = isSingleYear ? $"{startDate.Year}" : $"{startDate.Year}/{endDate.Year}",
					StartDate = startDate,
					EndDate = endDate,
					RoundsCount = RoundsCount,
					CreationUser = Constants.AutoGenerated,
					CreationDateTime = DateTime.Now,
				});
			}

			_context.SaveChanges();
		}

		private void AddPlayers()
		{
			var factory = new PlayerMockFactory();
			for (int i = 0; i < PlayersCount; i++)
			{
				_context.Players.Add(factory.CreateRandomPlayer());
			}
			_context.SaveChanges();
		}
	}

	internal class MatchStub
	{
		public Team Team1;
		public Team Team2;

		public override string ToString()
		{
			return $"{Team1.GetDisplayName()} - {Team2.GetDisplayName()}";
		}
	}
}