using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.DAL.EFCore.Mapping.Configurations
{
	public class ScoreConfiguration : IEntityTypeConfiguration<Score>
	{
		public void Configure(EntityTypeBuilder<Score> builder)
		{
			// - Table -
			builder.ToTable("Scores", "dbo");

			// - Key -
			builder.HasKey(s => s.Id);
			builder.Property(s => s.Id).UseSqlServerIdentityColumn();

			// - Columns -

			builder.Property(m => m.Minute).HasColumnType("tinyint").IsRequired();
			builder.Property(s => s.IsOG).HasColumnType("bit").IsRequired();
			builder.Property(s => s.IsPenalty).HasColumnType("bit").IsRequired();

			builder.HasOne(s => s.Match)
				.WithMany(m => m.Scores)
				.HasForeignKey(s => s.MatchId)
				.IsRequired();

			builder.HasOne(s => s.Team)
				.WithMany(t => t.ScoresFor)
				.HasForeignKey(s => s.TeamId)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			builder.HasOne(s => s.EnemyTeam)
				.WithMany(t => t.ScoresAgainst)
				.HasForeignKey(s => s.EnemyTeamId)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			builder.HasOne(s => s.Player)
				.WithMany(p => p.Scores)
				.HasForeignKey(s => s.PlayerId)
				.IsRequired();
		}
	}
}