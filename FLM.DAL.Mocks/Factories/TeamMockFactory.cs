﻿using FLM.Model.Entities;
using System;

namespace FLM.DAL.Mocks.Factories
{
	public class TeamMockFactory
	{
		private Random _random = new Random();

		public TeamMockFactory()
		{
		}

		public Team CreateRandomTeam()
		{
			var result = new Team();

			result.Name = Names[_random.Next(Names.Length)];
			result.City = Cities[_random.Next(Cities.Length)];

			result.FoundationYear = RandomDate().Year;

			result.CreationUser = Constants.AutoGenerated;
			result.CreationDateTime = DateTime.Now;

			return result;
		}

		private DateTime RandomDate()
		{
			DateTime start = new DateTime(1900, 1, 1);
			int range = (DateTime.Today - start).Days;
			return start.AddDays(_random.Next(range));
		}

		private string[] Names =
		{
			"FC",
			"Real",
			"United",
			"Athletics",
			"Olympic",
			"Arsenal",
			"Dynamo",
			"Torpedo",
			"Lokomotiv",
			"University",
			"Avengers",
			"Bears",
			"Bisons",
			"Bulls",
			"Eagles",
			"Falcons",
			"Giants",
			"Panthers",
			"Patriots",
			"Rangers",
			"Reds",
			"Royals",
			"Sharks",
			"Tigers",
			"Dragons",
		};

		private string[] Cities =
		{
			"Shanghai",
			"Beijing",
			"Delhi",
			"Istanbul",
			"Tokyo",
			"Guangzhou",
			"Mumbai",
			"Moscow",
			"São Paulo",
			"Jakarta",
			"Seoul",
			"Mexico",
			"London",
			"New York",
			"Bangkok",
			"Baghdad",
			"Rio de Janeiro",
			"Singapore",
			"Saint Petersburg",
			"Los Angeles",
			"Yokohama",
			"Berlin",
			"Madrid",
			"Buenos Aires",
			"Rome",
			"Paris",
			"Warsaw",
			"Minsk",
			"Oslo",
			"Kiev",
			"Budapest",
			"Munich",
			"Milan",
			"Prague",
		};
	}
}