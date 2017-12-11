using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace FLM.Client
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			// Disable telemetry https://github.com/aspnet/Home/issues/2051
			var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
			configuration.DisableTelemetry = true;

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				//app.UseDirectoryBrowser();
			}

			app.UseDefaultFiles();
			app.UseStaticFiles();

			// Handle client side routes
			app.Run(async (context) =>
			{
				context.Response.ContentType = "text/html";
				await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
			});
		}
	}
}