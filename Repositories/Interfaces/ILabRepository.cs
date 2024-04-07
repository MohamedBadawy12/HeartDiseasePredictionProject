using Database.Entities;
using HeartDiseasePrediction.ViewModel;

namespace Repositories.Interfaces
{
	public interface ILabRepository
	{
		Task<IEnumerable<Lab>> GetLabs();
		Task<Lab> GetLab(int id);
		Lab Get_Lab(int id);
		Task<NewLabDropDownViewMode> GetLabDropDownsValues();
		Task AddAsync(Lab lab);
		void Delete(Lab lab);
		bool DeleteLab(int id);
	}
}
