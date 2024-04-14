using Database.Entities;

namespace Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetDoctors();
        Task<IEnumerable<Doctor>> GetAvailableDoctors();
        Task<Doctor> GetDoctor(int id);
        //Task<Doctor> UpdateDoctor(Doctor doctor);
        Doctor FindDoctor(int id);
        //Task<NewDoctorDropDownViewModel> GetNewDoctorDropDownsValues();
        Task<IEnumerable<Doctor>> FilterDoctors(string search);
        Doctor Get_Doctor(int id);
        Task<Doctor> GetProfile(string userId);
        Task Add(Doctor doctor);
        void Delete(Doctor doctor);
        bool DeleteDoctor(int id);
    }
}
