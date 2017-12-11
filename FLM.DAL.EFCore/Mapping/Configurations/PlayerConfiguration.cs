using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.DAL.EFCore.Mapping.Configurations
{
	public class PlayerConfiguration : IEntityTypeConfiguration<Player>
	{
		public void Configure(EntityTypeBuilder<Player> builder)
		{
			// - Table -
			builder.ToTable("Players", "dbo");

			// - Key -
			builder.HasKey(p => p.Id);
			builder.Property(p => p.Id).UseSqlServerIdentityColumn();

			// - Columns -
			builder.Property(p => p.FirstName).HasColumnType("varchar(25)").IsRequired();
			builder.Property(p => p.LastName).HasColumnType("varchar(25)").IsRequired();
			builder.Property(p => p.DateOfBirth).HasColumnType("datetime").IsRequired();

			// - Columns: IAuditEntity -
			builder.Property(p => p.CreationUser).HasColumnType("varchar(25)").IsRequired();
			builder.Property(p => p.CreationDateTime).HasColumnType("datetime").IsRequired();
			builder.Property(p => p.LastUpdateUser).HasColumnType("varchar(25)");
			builder.Property(p => p.LastUpdateDateTime).HasColumnType("datetime");
		}
	}
}