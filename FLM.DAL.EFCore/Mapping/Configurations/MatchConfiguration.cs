using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.DAL.EFCore.Mapping.Configurations
{
	public class MatchConfiguration : IEntityTypeConfiguration<Match>
	{
		public void Configure(EntityTypeBuilder<Match> builder)
		{
			// - Table -
			builder.ToTable("Matches", "dbo");

			// - Key -
			builder.HasKey(m => m.Id);
			builder.Property(m => m.Id).UseSqlServerIdentityColumn();

			builder.HasIndex(m => new { m.LeagueId, m.Round, m.Team1Id });

			// - Columns -
			builder.Property(m => m.Date).HasColumnType("datetime").IsRequired();
			builder.Property(m => m.Round).HasColumnType("tinyint").IsRequired();
			builder.Property(m => m.Team1Score).HasColumnType("tinyint");
			builder.Property(m => m.Team2Score).HasColumnType("tinyint");

			builder.HasOne(m => m.League)
				.WithMany(l => l.Matches)
				.HasForeignKey(m => m.LeagueId)
				.IsRequired();

			builder.HasOne(m => m.Team1)
				.WithMany(t => t.HomeMatches)
				.HasForeignKey(m => m.Team1Id)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			builder.HasOne(m => m.Team2)
				.WithMany(t => t.AwayMatches)
				.HasForeignKey(m => m.Team2Id)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			// - Columns: IAuditEntity -
			builder.Property(m => m.CreationUser).HasColumnType("varchar(25)").IsRequired();
			builder.Property(m => m.CreationDateTime).HasColumnType("datetime").IsRequired();
			builder.Property(m => m.LastUpdateUser).HasColumnType("varchar(25)");
			builder.Property(m => m.LastUpdateDateTime).HasColumnType("datetime");
		}
	}
}