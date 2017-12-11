using FLM.DAL.Contracts;
using FLM.DAL.Contracts.Repositories;
using FLM.DAL.EFCore.Repositories.Base;
using FLM.Model.Entities;
using System.Threading.Tasks;

namespace FLM.DAL.EFCore.Repositories
{
	public class PlayerRepository : BaseEFRepository<Player>, IPlayerRepository
	{
		public PlayerRepository(FootballDbContext context, IUserResolver userResolver) : base(context, userResolver)
		{
		}
	}
}