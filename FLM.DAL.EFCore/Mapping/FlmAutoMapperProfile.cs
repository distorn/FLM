using AutoMapper;
using FLM.Model.Dto.League;
using FLM.Model.Dto.Match;
using FLM.Model.Dto.Player;
using FLM.Model.Dto.Team;
using FLM.Model.Entities;
using FLM.Model.Extensions;

namespace FLM.DAL.EFCore.Mapping
{
	public class FlmAutoMapperProfile : Profile
	{
		public FlmAutoMapperProfile()
		{
			// TODO: maybe add some extension methods grouping some common ForMember commands

			// - Player -

			CreateMap<Player, PlayerListItemDto>()
				.ForMember(dto => dto.FullName, m => m.MapFrom(entity => entity.GetDisplayName()))
				.ForMember(dto => dto.TeamId, m => m.MapFrom(entity => entity.TeamAssignment.Team.Id))
				.ForMember(dto => dto.TeamFullName, m => m.MapFrom(entity => entity.TeamAssignment.Team.GetDisplayName()));

			CreateMap<Player, PlayerDto>()
				.ForMember(dto => dto.TeamId, m => m.MapFrom(entity => entity.TeamAssignment.Team.Id))
				.ForMember(dto => dto.TeamFullName, m => m.MapFrom(entity => entity.TeamAssignment.Team.GetDisplayName()));

			CreateMap<Player, PlayerAuditDto>()
				.ForMember(dto => dto.TeamId, m => m.MapFrom(entity => entity.TeamAssignment.Team.Id))
				.ForMember(dto => dto.TeamFullName, m => m.MapFrom(entity => entity.TeamAssignment.Team.GetDisplayName()));

			CreateMap<Player, PlayerLookupDto>()
				.ForMember(dto => dto.FullName, m => m.MapFrom(entity => entity.GetDisplayName()));

			// - League -

			CreateMap<League, LeagueListItemDto>()
				.ForMember(dto => dto.TeamsCount, m => m.MapFrom(entity => entity.Teams.Count));
			CreateMap<League, LeagueDto>();
			CreateMap<League, LeagueAuditDto>();

			CreateMap<TeamLeagueAssignment, LeagueTeamDto>()
				.ForMember(dto => dto.FullName, m => m.MapFrom(entity => entity.Team.GetDisplayName()));

			CreateMap<TeamTableStanding, TeamTableStandingDto>()
				.ForMember(dto => dto.TeamFullName, m => m.MapFrom(entity => entity.Team.GetDisplayName()));

			// - Team -

			CreateMap<Team, TeamListItemDto>()
				.ForMember(dto => dto.PlayersCount, m => m.MapFrom(entity => entity.Players.Count));

			CreateMap<Team, TeamDto>();
			CreateMap<Team, TeamAuditDto>();
			CreateMap<Team, TeamLookupDto>()
				.ForMember(dto => dto.FullName, m => m.MapFrom(entity => entity.GetDisplayName()));

			CreateMap<PlayerTeamAssignment, TeamPlayerDto>()
				.ForMember(tp => tp.FirstName, m => m.MapFrom(entity => entity.Player.FirstName))
				.ForMember(tp => tp.LastName, m => m.MapFrom(entity => entity.Player.LastName))
				.ForMember(tp => tp.DateOfBirth, m => m.MapFrom(entity => entity.Player.DateOfBirth));

			// - Match -

			CreateMap<Match, MatchListItemDto>()
				.ForMember(m => m.LeagueFullName, m => m.MapFrom(entity => entity.League.GetDisplayName()))
				.ForMember(m => m.Team1FullName, m => m.MapFrom(entity => entity.Team1.GetDisplayName()))
				.ForMember(m => m.Team2FullName, m => m.MapFrom(entity => entity.Team2.GetDisplayName()));

			CreateMap<Match, MatchDto>()
				.ForMember(m => m.LeagueFullName, m => m.MapFrom(entity => entity.League.GetDisplayName()))
				.ForMember(m => m.Team1FullName, m => m.MapFrom(entity => entity.Team1.GetDisplayName()))
				.ForMember(m => m.Team2FullName, m => m.MapFrom(entity => entity.Team2.GetDisplayName()));

			CreateMap<Match, MatchAuditDto>()
				.ForMember(m => m.LeagueFullName, m => m.MapFrom(entity => entity.League.GetDisplayName()))
				.ForMember(m => m.Team1FullName, m => m.MapFrom(entity => entity.Team1.GetDisplayName()))
				.ForMember(m => m.Team2FullName, m => m.MapFrom(entity => entity.Team2.GetDisplayName()));

			CreateMap<Score, ScoreDto>()
				.ForMember(m => m.PlayerFullName, m => m.MapFrom(entity => entity.Player.GetDisplayName()));
		}
	}
}