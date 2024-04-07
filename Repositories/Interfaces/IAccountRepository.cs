using Database.Entities;

namespace Repositories.Interfaces
{
	public interface IAccountRepository
	{
		void UpdateProfile(ApplicationUser user);
	}
}
