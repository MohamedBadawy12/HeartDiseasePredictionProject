using Database.Entities;
using Repositories.Interfaces;

namespace Repositories
{
	public class AccountRepository : IAccountRepository
	{
		private readonly AppDbContext _context;
		public AccountRepository(AppDbContext context)
		{
			_context = context;
		}
		public void UpdateProfile(ApplicationUser user) =>
			 _context.Users.Update(user);
	}
}
