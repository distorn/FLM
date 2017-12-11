using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.DAL.EFCore.Mapping.Configurations
{
	public class TeamTableStandingConfiguration : IEntityTypeConfiguration<TeamTableStanding>
	{
		public void Configure(EntityTypeBuilder<TeamTableStanding> builder)
		{
			// - Table -

			builder.ToTable("TableStandings", "dbo");

			// - Key -

			builder.Ignore(tts => tts.Id);
			builder.HasKey(tts => new { tts.LeagueId, tts.TeamId });

			// - Columns -

			builder.Property(tts => tts.Position).HasColumnType("tinyint").IsRequired();

			builder.Property(tts => tts.MatchesPlayed).HasColumnType("tinyint").IsRequired();
			builder.Property(tts => tts.MatchesWon).HasColumnType("tinyint").IsRequired();
			builder.Property(tts => tts.MatchesDrawn).HasColumnType("tinyint").IsRequired();
			builder.Property(tts => tts.MatchesLost).HasColumnType("tinyint").IsRequired();

			builder.Property(tts => tts.GoalsFor).HasColumnType("int").IsRequired();
			builder.Property(tts => tts.GoalsAgainst).HasColumnType("int").IsRequired();

			builder.Property(tts => tts.Points).HasColumnType("int").IsRequired();

			// - Navigation Properties -

			builder.HasOne(tts => tts.Team)
				.WithMany(t => t.Standings)
				.HasForeignKey(tts => tts.TeamId)
				.IsRequired();

			builder.HasOne(tts => tts.League)
				.WithMany(l => l.Standings)
				.HasForeignKey(tts => tts.LeagueId)
				.IsRequired();
		}
	}
}