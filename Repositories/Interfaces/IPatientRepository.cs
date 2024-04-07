using Database.Entities;

namespace Repositories.Interfaces
{
	public interface IPatientRepository
	{
		Task<IEnumerable<Patient>> GetPatients();
		Task<IEnumerable<Patient>> GetRecentPatients();
		Task<IEnumerable<Patient>> FilterPatients(string search);
		Task<Patient> GetPatient(long ssn);
		Task<Patient> GetProfile(string userId);
		Patient Get_Patient(long ssn);
		Task Add(Patient patient);
		void Remove(Patient patient);
		bool Delete(long ssn);
	}
}
