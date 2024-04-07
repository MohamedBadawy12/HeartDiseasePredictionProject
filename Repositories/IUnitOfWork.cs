using Repositories.Interfaces;

namespace Repositories
{
	public interface IUnitOfWork
	{
		IDoctorRepository Doctors { get; }
		IPatientRepository Patients { get; }
		IMedicalAnalystRepository medicalAnalysts { get; }
		IReciptionistRepository reciptionists { get; }
		IPrescriptionRepository prescriptions { get; }
		IAppointmentRepository appointments { get; }
		ILabRepository labs { get; }
		IAccountRepository accounts { get; }
		Task Complete();
	}
}
