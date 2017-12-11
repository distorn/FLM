using FLM.DAL.EFCore;
using FLM.DAL.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FLM.API.Services
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FootballDbContext>
	{
		public FootballDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var builder = new DbContextOptionsBuilder<FootballDbContext>();
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			builder.UseSqlServer(connectionString);

			return new FootballDbContext(builder.Options, new FlmEntityMapper());
		}
	}
}