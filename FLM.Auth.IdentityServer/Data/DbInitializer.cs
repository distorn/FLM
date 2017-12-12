using FLM.Auth.IdentityServer.Models;
using FLM.Model.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FLM.Auth.IdentityServer.Data
{
	public class DbInitializer : IDbInitializer
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceProvider _serviceProvider;

		public DbInitializer(
			ApplicationDbContext context,
			IServiceProvider serviceProvider
			)
		{
			_context = context;
			_serviceProvider = serviceProvider;
		}

		public async void Initialize()
		{
			_context.Database.Migrate();

			//create database schema if none exists
			_context.Database.EnsureCreated();

			//If there is already an Administrator role, abort
			if (_context.Roles.Any(r => r.Name == UserRoles.Administrator)) return;

			using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
				var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

				//Create the Administartor Role
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.Administrator));

				// Add Admin
				await _userManager.CreateAsync(new ApplicationUser
				{
					UserName = "admin@test.com",
					Email = "admin@test.com",
					EmailConfirmed = true
				}, "admin");
				await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("admin@test.com"), UserRoles.Administrator);

				// Add 2 users
				await _userManager.CreateAsync(new ApplicationUser
				{
					UserName = "user1@test.com",
					Email = "user1@test.com",
					EmailConfirmed = true
				}, "user1");

				await _userManager.CreateAsync(new ApplicationUser
				{
					UserName = "user2@test.com",
					Email = "user2@test.com",
					EmailConfirmed = true
				}, "user2");
			}
		}
	}
}