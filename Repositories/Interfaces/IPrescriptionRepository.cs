using Database.Entities;
using Repositories.DropDownViewModel;
using Repositories.ViewModel;

namespace Repositories.Interfaces
{
	public interface IPrescriptionRepository
	{
		Task AddPrescriptionAsync(PrescriptionViewModel model);
		Task<List<Prescription>> GetPrescriptionByUserId(string userId, string userRole);
		Task<List<Prescription>> GetPrescriptionByEmail(string Email, string userRole);
		Task<List<Prescription>> SearchPrescriptionByUserId(string userId, string userRole, DateTime? date, long? ssn);
		Task<List<Prescription>> SearchPrescriptionByEmail(string Email, string userRole, DateTime? date, string? DoctorName);
		Task<IEnumerable<Prescription>> GetPrescriptions();
		Task<List<Prescription>> GetPrescriptionsByUserSSN(long ssn);
		Task<IEnumerable<Prescription>> FilterPrescriptions(DateTime? date);
		Task<IEnumerable<Prescription>> FilterPrescriptions(long? search);
		Task<Prescription> GetPrescription(int id);
		Prescription Get_Prescription(int id);
		Task<DoctorDropDownViewMode> GetDoctorDropDownsValues();
		Task AddAsync(Prescription prescription);
		void Remove(Prescription prescription);
		bool Delete(int id);
	}
}
