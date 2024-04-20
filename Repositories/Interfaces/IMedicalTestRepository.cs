using Database.Entities;

namespace Repositories.Interfaces
{
    public interface IMedicalTestRepository
    {
        Task<IEnumerable<MedicalTest>> GetMedicalTests();
        Task AddAsync(MedicalTest medicalTest);
        Task<List<MedicalTest>> GetMedicalTestsByUserId(string userId, string userRole);
        Task<List<MedicalTest>> GetMedicalTestsByMedicalId(string userId, string userRole);
        Task<List<MedicalTest>> SearchMedicalTestsByUserId(string userId, string userRole, DateTime? date, long? ssn);
        Task<List<MedicalTest>> GetMedicalTestsByEmail(string Email, string userRole);
        Task<List<MedicalTest>> GetMedicalTestsByPatientEmail(string Email, string userRole);
        Task<List<MedicalTest>> SearchMedicalTestsByEmail(string Email, string userRole, DateTime? date);
        Task<MedicalTest> GetMedicalTest(int id);
        MedicalTest Get_MedicalTest(int id);
        void Remove(MedicalTest medicalTest);
        bool Delete(int id);
    }
}
