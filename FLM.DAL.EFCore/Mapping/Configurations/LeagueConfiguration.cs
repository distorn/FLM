using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.DAL.EFCore.Mapping.Configurations
{
	public class LeagueConfiguration : IEntityTypeConfiguration<League>
	{
		public void Configure(EntityTypeBuilder<League> builder)
		{
			// - Table -
			builder.ToTable("Leagues", "dbo");

			// - Key -
			builder.HasKey(l => l.Id);
			builder.Property(l => l.Id).UseSqlServerIdentityColumn();

			// - Columns -
			builder.Property(l => l.Name).HasColumnType("varchar(25)").IsRequired();
			builder.Property(l => l.Season).HasColumnType("varchar(25)").IsRequired();
			builder.Property(l => l.StartDate).HasColumnType("datetime").IsRequired();
			builder.Property(l => l.EndDate).HasColumnType("datetime").IsRequired();
			builder.Property(l => l.RoundsCount).HasColumnType("tinyint").IsRequired();

			// - Columns: IAuditEntity -
			builder.Property(l => l.CreationUser).HasColumnType("varchar(25)").IsRequired();
			builder.Property(l => l.CreationDateTime).HasColumnType("datetime").IsRequired();
			builder.Property(l => l.LastUpdateUser).HasColumnType("varchar(25)");
			builder.Property(l => l.LastUpdateDateTime).HasColumnType("datetime");

			// - League : Teams -

			builder.HasMany(l => l.Teams);
		}
	}
}