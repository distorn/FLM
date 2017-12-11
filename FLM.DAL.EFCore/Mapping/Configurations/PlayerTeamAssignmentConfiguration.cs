using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.DAL.EFCore.Mapping.Configurations
{
	public class PlayerTeamAssignmentConfiguration : IEntityTypeConfiguration<PlayerTeamAssignment>
	{
		public void Configure(EntityTypeBuilder<PlayerTeamAssignment> builder)
		{
			// - Table -

			builder.ToTable("PlayerTeamAssignments", "dbo");

			builder.Ignore(pta => pta.Id);
			builder.HasKey(pta => new { pta.TeamId, pta.Number });

			// - Columns -

			builder.Property(pta => pta.Number).HasColumnType("tinyint").IsRequired();

			// - Navigation Properties -

			builder.HasOne(pta => pta.Team)
				.WithMany(t => t.Players)
				.HasForeignKey(pta => pta.TeamId)
				.IsRequired();

			builder.HasOne(pta => pta.Player)
				.WithOne(p => p.TeamAssignment)
				.HasForeignKey<PlayerTeamAssignment>(pta => pta.PlayerId)
				.IsRequired();
		}
	}
}