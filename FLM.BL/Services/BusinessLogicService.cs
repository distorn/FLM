using AutoMapper;
using FLM.BL.Contracts;
using FLM.DAL.Contracts;
using FLM.DAL.Contracts.Repositories;
using FLM.Model.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FLM.BL.Services
{
	public abstract class BusinessLogicService : IBusinessLogicService
	{
		protected IServiceProvider ServiceProvider;
		protected ILogger Logger;
		protected Boolean Disposed;

		private IFootballDbContext _dbContext;
		private IMapper _mapper;
		private IUserInfo _userInfo;

		private IPlayerRepository _playerRepository;
		private ILeagueRepository _leagueRepository;
		private ITeamRepository _teamRepository;
		private IMatchRepository _matchRepository;

		public BusinessLogicService(IServiceProvider serviceProvider, ILogger logger = null)
		{
			ServiceProvider = serviceProvider;
			Logger = logger;
		}

		public void Dispose()
		{
			if (!Disposed)
			{
				_dbContext?.Dispose();
				Disposed = true;
			}
		}

		protected bool ShouldIncludeAuditData()
		{
			return UserInfo != null && UserInfo.ShowAuditData;
		}

		// - Services resolved on first demand -

		protected IFootballDbContext DbContext
		{
			get { return _dbContext ?? (_dbContext = ServiceProvider.GetService<IFootballDbContext>()); }
		}

		protected IMapper Mapper
		{
			get { return _mapper ?? (_mapper = ServiceProvider.GetService<IMapper>()); }
		}

		protected IUserInfo UserInfo
		{
			get { return _userInfo ?? (_userInfo = ServiceProvider.GetService<IUserResolver>()?.GetUser()); }
		}

		// - Repositories -

		protected IPlayerRepository PlayerRepository
		{
			get { return _playerRepository ?? (_playerRepository = ServiceProvider.GetService<IPlayerRepository>()); }
		}

		protected ILeagueRepository LeagueRepository
		{
			get { return _leagueRepository ?? (_leagueRepository = ServiceProvider.GetService<ILeagueRepository>()); }
		}

		protected ITeamRepository TeamRepository
		{
			get { return _teamRepository ?? (_teamRepository = ServiceProvider.GetService<ITeamRepository>()); }
		}

		protected IMatchRepository MatchRepository
		{
			get { return _matchRepository ?? (_matchRepository = ServiceProvider.GetService<IMatchRepository>()); }
		}
	}
}