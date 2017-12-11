using FLM.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.DAL.EFCore.Mapping.Configurations
{
	public class TeamLeagueAssignmentConfiguration : IEntityTypeConfiguration<TeamLeagueAssignment>
	{
		public void Configure(EntityTypeBuilder<TeamLeagueAssignment> builder)
		{
			// - Table -

			builder.ToTable("TeamLeagueAssignments", "dbo");

			// - Key -

			builder.Ignore(tla => tla.Id);
			builder.HasKey(tla => new { tla.LeagueId, tla.TeamId });

			// - Navigation Properties -

			builder.HasOne(tla => tla.Team)
				.WithMany(t => t.Leagues)
				.HasForeignKey(tla => tla.TeamId)
				.IsRequired();

			builder.HasOne(tla => tla.League)
				.WithMany(t => t.Teams)
				.HasForeignKey(tla => tla.LeagueId)
				.IsRequired();
		}
	}
}