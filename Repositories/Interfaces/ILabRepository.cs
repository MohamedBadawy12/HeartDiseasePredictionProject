using Database.Entities;
using HeartDiseasePrediction.ViewModel;

namespace Repositories.Interfaces
{
    public interface ILabRepository
    {
        Task<IEnumerable<Lab>> GetLabs();
        Task<Lab> GetLab(int id);
        Task<IEnumerable<Lab>> SearchForLab(string name, string location);
        Lab Get_Lab(int id);
        Task<NewLabDropDownViewMode> GetLabDropDownsValues();
        Task AddAsync(Lab lab);
        void Delete(Lab lab);
        bool DeleteLab(int id);
    }
}
