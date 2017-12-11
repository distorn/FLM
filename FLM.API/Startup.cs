using AutoMapper;
using FLM.API.Security;
using FLM.API.Services;
using FLM.BL.Contracts;
using FLM.BL.Services;
using FLM.DAL.Contracts;
using FLM.DAL.Contracts.Repositories;
using FLM.DAL.EFCore;
using FLM.DAL.EFCore.Mapping;
using FLM.DAL.EFCore.Repositories;
using FLM.DAL.Mocks;
using FLM.Model.User;
using IdentityServer4.AccessTokenValidation;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FLM.API
{
	public class Startup
	{
		private const string CORS_POLICY_ALLOW_ALL = "AllowAll";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// - Cors -

			services.AddCors(options =>
			{
				options.AddPolicy(CORS_POLICY_ALLOW_ALL,
					builder =>
					{
						builder
							.AllowAnyOrigin()
							.AllowAnyHeader()
							.AllowAnyMethod();
					});
			});

			// - Auth -
			// TODO: move to config
			services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
				.AddIdentityServerAuthentication(options =>
				{
					options.Authority = "https://localhost:44335/";
					options.ApiName = "flm.api";
					options.ApiSecret = "flm.api.s3cr3t";
				});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(Policies.CanEditData, policyUser =>
				{
					policyUser.RequireClaim("role", UserRoles.Administrator);
				});
			});

			// - Database -

			var dataLayerAssembly = typeof(FootballDbContext).GetTypeInfo().Assembly;
			services.AddDbContext<FootballDbContext>(
				options => options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection"),
					sqlServerOptions => sqlServerOptions.MigrationsAssembly(dataLayerAssembly.GetName().Name)
				)
			);
			// trick for ability to inject DbContext in services/controllers via interface
			services.AddScoped<IFootballDbContext>(provider => provider.GetService<FootballDbContext>());
			services.AddScoped<IEntityMapper, FlmEntityMapper>();
			services.AddScoped<IDbInitializer, MockDbInitializer>();

			services.AddAutoMapper(dataLayerAssembly);

			// - API Services -

			services.AddScoped<IUserResolver, UserResolver>();

			services.AddScoped<IPlayerRepository, PlayerRepository>();
			services.AddScoped<IPlayerService, PlayerService>();

			services.AddScoped<ILeagueRepository, LeagueRepository>();
			services.AddScoped<ILeagueService, LeagueService>();

			services.AddScoped<ITeamRepository, TeamRepository>();
			services.AddScoped<ITeamService, TeamService>();

			services.AddScoped<IMatchRepository, MatchRepository>();
			services.AddScoped<IMatchService, MatchService>();

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer)
		{
			app.UseCors(CORS_POLICY_ALLOW_ALL);

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				// Disable telemetry. Based on https://github.com/aspnet/Home/issues/2051
				var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
				if (configuration != null)
				{
					configuration.DisableTelemetry = true;
				}
			}

			dbInitializer?.Initialize();

			app.UseAuthentication();
			app.UseMvc();
		}
	}
}