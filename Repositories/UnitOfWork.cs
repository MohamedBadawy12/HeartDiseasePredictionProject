using Database.Entities;
using Repositories.Interfaces;

namespace Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _context;
		public IDoctorRepository Doctors { get; private set; }
		public IPatientRepository Patients { get; private set; }
		public IMedicalAnalystRepository medicalAnalysts { get; private set; }
		public IReciptionistRepository reciptionists { get; private set; }
		public IPrescriptionRepository prescriptions { get; private set; }
		public ILabRepository labs { get; private set; }
		public IAppointmentRepository appointments { get; private set; }
		public IAccountRepository accounts { get; private set; }

		public UnitOfWork(AppDbContext context)
		{
			_context = context;
			Patients = new PatientRepository(context);
			Doctors = new DoctorRepository(context);
			medicalAnalysts = new MedicalAnalystRepository(context);
			reciptionists = new ReciptionistRepository(context);
			appointments = new AppointmentRepository(context);
			accounts = new AccountRepository(context);
			labs = new LabRepository(context);
			prescriptions = new PrescriptionRepository(context);
		}

		public async Task Complete() => await _context.SaveChangesAsync();
	}
}
