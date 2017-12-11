using FLM.Auth.IdentityServer.Data;
using FLM.Auth.IdentityServer.Models;
using FLM.Auth.IdentityServer.Services;
using IdentityServer4.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FLM.Auth.IdentityServer
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

			// - Database -

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddScoped<IDbInitializer, DbInitializer>();

			// - Identity -

			services.AddAuthentication();

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddMvc();

			// Add application services.
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<IProfileService, FlmProfileService>();

			// Set not very strict password settings just for quick testing purpose
			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequiredLength = 5;
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
			});

			/* // TODO: Use real certificate for production
			var servicesProvider = services.BuildServiceProvider();
			var env = servicesProvider.GetService<IHostingEnvironment>();
			var cert = new X509Certificate2(Path.Combine(env.ContentRootPath, "sertificate.pfx"), "");
			*/

			services.AddIdentityServer()
				//.AddSigningCredential(cert)
				.AddDeveloperSigningCredential()
				.AddInMemoryIdentityResources(Config.GetIdentityResources())
				.AddInMemoryApiResources(Config.GetApiResources())
				.AddInMemoryClients(Config.GetClients())
				.AddAspNetIdentity<ApplicationUser>()
				.AddProfileService<FlmProfileService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer)
		{
			app.UseCors(CORS_POLICY_ALLOW_ALL);

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
				app.UseDatabaseErrorPage();
				// Disable telemetry. Based on https://github.com/aspnet/Home/issues/2051
				var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
				if (configuration != null)
				{
					configuration.DisableTelemetry = true;
				}
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			app.UseIdentityServer();
			app.UseAuthentication();

			dbInitializer?.Initialize();

			app.UseMvcWithDefaultRoute();
		}
	}
}