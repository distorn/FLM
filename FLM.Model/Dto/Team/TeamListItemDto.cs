namespace FLM.Model.Dto.Team
{
	public class TeamListItemDto
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string City { get; set; }
		public int FoundationYear { get; set; }

		public int PlayersCount { get; set; }
	}
}