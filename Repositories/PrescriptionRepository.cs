using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.DropDownViewModel;
using Repositories.Interfaces;
using Repositories.ViewModel;

namespace Repositories
{
	public class PrescriptionRepository : IPrescriptionRepository
	{
		private readonly AppDbContext _context;
		public PrescriptionRepository(AppDbContext context)
		{
			_context = context;
		}
		public async Task AddAsync(Prescription prescription) =>
			await _context.Prescriptions.AddAsync(prescription);

		public async Task<IEnumerable<Prescription>> FilterPrescriptions(DateTime? date)
		{
			var prescriptions = await GetPrescriptions();
			if ((date.HasValue && date != null) || date == DateTime.MinValue)
			{
				prescriptions = await _context.Prescriptions.Where(x => date.HasValue && x.date.Year == date.Value.Year && x.date.Month == date.Value.Month && x.date.Day == date.Value.Day).Include(d => d.Doctor).Include(p => p.Patient).ToListAsync();
			}
			return prescriptions;
		}

		public async Task<DoctorDropDownViewMode> GetDoctorDropDownsValues()
		{
			var data = new DoctorDropDownViewMode()
			{
				doctors = await _context.Doctors.OrderBy(a => a.Name).ToListAsync(),
			};
			return data;
		}


		public async Task<Prescription> GetPrescription(int id) =>
			 await _context.Prescriptions.Include(d => d.Doctor).FirstOrDefaultAsync(p => p.Id == id);

		public async Task<List<Prescription>> GetPrescriptionByUserId(string userId, string userRole)
		{
			IQueryable<Prescription> prescriptionsQuery = _context.Prescriptions;

			if (userRole == "Doctor")
			{
				prescriptionsQuery = prescriptionsQuery.Include(a => a.Patient);
			}
			var prescriptions = await prescriptionsQuery
				 .Where(a => (userRole == "Doctor" && a.ApDoctorId == userId))
				.Select(a => new Prescription
				{
					Id = a.Id,
					date = a.date,
					MedicineName = a.MedicineName,
					DoctorEmail = a.DoctorEmail,
					PatientEmail = a.PatientEmail,
					PatientSSN = a.PatientSSN,
					DoctorId = a.DoctorId,
					ApDoctorId = a.ApDoctorId,

					Patient = userRole == "Doctor" ? new Patient
					{
						SSN = a.PatientSSN,
						Insurance_No = a.Patient.Insurance_No,
						User = a.Patient.User,
					} : null
				})
				.ToListAsync();

			return prescriptions;
		}

		public async Task<IEnumerable<Prescription>> GetPrescriptions() =>
			 await _context.Prescriptions.Include(d => d.Doctor).ToListAsync();

		public async Task<List<Prescription>> GetPrescriptionsByUserSSN(long ssn)
		{
			var prescriptions = await _context.Prescriptions.Include(d => d.Doctor).Where(n => n.PatientSSN == ssn).ToListAsync();
			//if (userRole == "User")
			//{
			//	prescriptions = prescriptions.Where(n => n.PatientSSN == ssn).ToList();
			//}
			return prescriptions;
		}

		public Prescription Get_Prescription(int id) =>
			 _context.Prescriptions
			.Include(d => d.Doctor)
			.Include(p => p.Patient)
			.Include(D => D.Doctorr)
			//.Include(P => P.Patientt)
			.FirstOrDefault(i => i.Id == id);

		public void Remove(Prescription prescription) =>
			_context.Prescriptions.Remove(prescription);

		public async Task AddPrescriptionAsync(PrescriptionViewModel model)
		{
			var prescription = new Prescription()
			{
				ApDoctorId = model.ApDoctorId,
				DoctorEmail = model.DoctorEmail,
				PatientEmail = model.PatientEmail,
				PatientSSN = model.PatientSSN,
				DoctorId = model.DoctorId,
				date = DateTime.Now,
				MedicineName = model.MedicineName,
			};
			await _context.Prescriptions.AddAsync(prescription);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Prescription>> GetPrescriptionByEmail(string Email, string userRole)
		{
			IQueryable<Prescription> prescriptionsQuery = _context.Prescriptions;

			if (userRole == "User")
			{
				prescriptionsQuery = prescriptionsQuery.Include(a => a.Doctorr);
			}
			var prescriptions = await prescriptionsQuery
				.Where(a => (userRole == "Doctor" && a.DoctorEmail == Email) ||
							(userRole == "User" && a.PatientEmail == Email))
				.Select(a => new Prescription
				{
					Id = a.Id,
					date = a.date,
					MedicineName = a.MedicineName,
					DoctorEmail = a.DoctorEmail,
					PatientSSN = a.PatientSSN,
					DoctorId = a.DoctorId,
					PatientEmail = a.PatientEmail,
					ApDoctorId = a.ApDoctorId,

					Doctorr = userRole == "User" ? new ApplicationUser
					{
						Id = a.ApDoctorId,
						FirstName = a.Doctorr.FirstName,
						LastName = a.Doctorr.LastName,
						Name = a.Doctorr.Name,
						PhoneNumber = a.Doctorr.PhoneNumber,
						Email = a.Doctorr.Email,
						Location = a.Doctorr.Location,
						Price = a.Doctorr.Price,
						BirthDate = a.Doctorr.BirthDate,
						Gender = a.Doctorr.Gender,
					} : null
				})
				.ToListAsync();

			return prescriptions;
		}

		public bool Delete(int id)
		{
			var isDeleted = false;

			var prescription = _context.Prescriptions
			.Include(d => d.Doctor)
			.Include(p => p.Patient)
			.Include(D => D.Doctorr)
			.FirstOrDefault(i => i.Id == id);

			if (prescription is null)
				return isDeleted;

			_context.Remove(prescription);
			var effectedRows = _context.SaveChanges();

			if (effectedRows > 0)
			{
				isDeleted = true;
			}

			return isDeleted;
		}

		public async Task<List<Prescription>> SearchPrescriptionByUserId(string userId, string userRole, DateTime? date, long? ssn)
		{
			long searchSSN = ssn ?? 0;
			var prescription = await GetPrescriptionByUserId(userId, userRole);
			if ((date.HasValue && date != null) || date == DateTime.MinValue || (ssn != null && ssn != 0))
			{
				prescription = await _context.Prescriptions.Where(x => (date.HasValue && x.date.Year == date.Value.Year && x.date.Month == date.Value.Month && x.date.Day == date.Value.Day)
				|| (x.PatientSSN == searchSSN)).Include(d => d.Doctorr).Include(p => p.Patient).ToListAsync();
			}
			return prescription;
		}

		public async Task<List<Prescription>> SearchPrescriptionByEmail(string Email, string userRole, DateTime? date, string? DoctorName)
		{
			var prescription = await GetPrescriptionByEmail(Email, userRole);
			if ((date.HasValue && date != null) || date == DateTime.MinValue || DoctorName != null)
			{
				prescription = await _context.Prescriptions.Where(x => (date.HasValue && x.date.Year == date.Value.Year && x.date.Month == date.Value.Month && x.date.Day == date.Value.Day && userRole == "User" && x.PatientEmail == Email)
				|| (x.Doctorr.Name.Contains(DoctorName) && userRole == "User" && x.PatientEmail == Email)).Include(d => d.Doctorr).Include(d => d.Doctor).Include(p => p.Patient).ToListAsync();
			}
			return prescription;
		}

		public async Task<IEnumerable<Prescription>> FilterPrescriptions(long? search)
		{
			var prescriptions = await GetPrescriptions();
			if (search != 0)
			{
				prescriptions = await _context.Prescriptions.Where(x => x.PatientSSN == search).Include(d => d.Doctor).Include(p => p.Patient).ToListAsync();
			}
			return prescriptions;
		}
	}
}
