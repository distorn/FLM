using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.DAL.EFCore.Mapping.Configurations
{
	public class TeamConfiguration : IEntityTypeConfiguration<Team>
	{
		public void Configure(EntityTypeBuilder<Team> builder)
		{
			// - Table -
			builder.ToTable("Teams", "dbo");

			// - Key -
			builder.HasKey(t => t.Id);
			builder.Property(t => t.Id).UseSqlServerIdentityColumn();

			// - Columns -
			builder.Property(t => t.Name).HasColumnType("varchar(25)").IsRequired();
			builder.Property(t => t.City).HasColumnType("varchar(25)").IsRequired();
			builder.Property(t => t.FoundationYear).HasColumnType("int").IsRequired();

			// - Columns: IAuditEntity -
			builder.Property(t => t.CreationUser).HasColumnType("varchar(25)").IsRequired();
			builder.Property(t => t.CreationDateTime).HasColumnType("datetime").IsRequired();
			builder.Property(t => t.LastUpdateUser).HasColumnType("varchar(25)");
			builder.Property(t => t.LastUpdateDateTime).HasColumnType("datetime");

			// - Navigation Properties -
			builder.HasMany(t => t.Players);
			builder.HasMany(l => l.Leagues);
		}
	}
}