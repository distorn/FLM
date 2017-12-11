using FLM.DAL.Contracts;
using FLM.DAL.EFCore.Mapping;
using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace FLM.DAL.EFCore
{
	public class FootballDbContext : DbContext, IFootballDbContext
	{
		public IEntityMapper EntityMapper { get; }

		public DbSet<Player> Players { get; set; }
		public DbSet<League> Leagues { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Match> Matches { get; set; }
		public DbSet<Score> Scores { get; set; }

		public DbSet<PlayerTeamAssignment> PlayerTeamAssignments { get; set; }
		public DbSet<TeamLeagueAssignment> TeamLeagueAssignments { get; set; }

		public DbSet<TeamTableStanding> TableStandings { get; set; }

		public FootballDbContext(DbContextOptions<FootballDbContext> options, IEntityMapper mapper = null) : base(options)
		{
			EntityMapper = mapper;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			EntityMapper?.MapEntities(modelBuilder);
		}
	}
}