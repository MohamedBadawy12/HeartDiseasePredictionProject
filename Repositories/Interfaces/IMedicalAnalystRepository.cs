using Database.Entities;

namespace Repositories.Interfaces
{
	public interface IMedicalAnalystRepository
	{
		Task<IEnumerable<MedicalAnalyst>> GetMedicalAnalysts();
		Task<MedicalAnalyst> GetMedicalAnalyst(int id);
		MedicalAnalyst Get_MedicalAnalyst(int id);
		Task<IEnumerable<MedicalAnalyst>> FilterMedicalAnalyst(string search);
		Task<MedicalAnalyst> GetProfile(string userId);
		Task Add(MedicalAnalyst medicalAnalyst);
		void Remove(MedicalAnalyst medicalAnalyst);
		bool Delete(int id);
	}
}
