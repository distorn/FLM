using System;

namespace FLM.API.ViewModels
{
	public class MatchesFilterViewModel
	{
		public int? LeagueId { get; set; }
		public int? TeamId { get; set; }
		public DateTime? MinDate { get; set; }
		public DateTime? MaxDate { get; set; }
	}
}