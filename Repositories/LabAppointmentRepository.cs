using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class LabAppointmentRepository : ILabAppointmentRepository
    {
        private readonly AppDbContext _context;
        public LabAppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAcceptOrCancelAsync(AcceptAndCancelLabAppointment appointment) =>
             await _context.AcceptAndCancelLabAppointments.AddAsync(appointment);


        public async Task AddAsync(LabAppointment appointment) =>
             await _context.LabAppointments.AddAsync(appointment);

        public bool Canceled(int id)
        {
            var isDeleted = false;

            var appointment = _context.LabAppointments
            .Include(d => d.Patient)
            .Include(p => p.Patientt)
            .Include(P => P.Lab)
            .FirstOrDefault(i => i.Id == id);

            if (appointment is null)
                return isDeleted;

            _context.Remove(appointment);
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;
            }

            return isDeleted;
        }

        public async Task<List<AcceptAndCancelLabAppointment>> GetAcceptAndCancelLabAppointment(string userId, string userRole)
        {
            IQueryable<AcceptAndCancelLabAppointment> appointmentsQuery = _context.AcceptAndCancelLabAppointments;

            if (userRole == "User")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Lab);
            }
            var appointments = await appointmentsQuery
                .Where(a => (userRole == "User" && a.PatientID == userId))
                .Select(a => new AcceptAndCancelLabAppointment
                {
                    Id = a.Id,
                    Date = a.Date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    LabEmail = a.LabEmail,
                    PatientEmail = a.PatientEmail,
                    PatientSSN = a.PatientSSN,
                    PatientImage = a.PatientImage,
                    PhoneNumber = a.PhoneNumber,
                    IsAccepted = a.IsAccepted,
                    LabId = a.LabId,

                    Lab = userRole == "User" ? new Lab
                    {
                        Id = a.Id,
                        Name = a.Lab.Name,
                        Price = a.Lab.Price,
                        Location = a.Lab.Location,
                        Zone = a.Lab.Zone,
                        StartTime = a.Lab.StartTime,
                        EndTime = a.Lab.EndTime,
                        LabImage = a.Lab.LabImage,
                        User = a.Lab.User,
                    } : null,
                })
                .ToListAsync();

            return appointments;
        }

        public async Task<AcceptAndCancelLabAppointment> GetAcceptAppointment(int id) =>
         await _context.AcceptAndCancelLabAppointments
           .Include(d => d.Patient)
            .Include(p => p.Patientt)
            .Include(l => l.Lab)
            .FirstOrDefaultAsync(i => i.Id == id);

        public async Task<List<AcceptAndCancelLabAppointment>> GetAcceptLabAppointmentByMedical(string Email, string userRole)
        {
            IQueryable<AcceptAndCancelLabAppointment> appointmentsQuery = _context.AcceptAndCancelLabAppointments;

            if (userRole == "MedicalAnalyst")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Patientt);
            }
            var appointments = await appointmentsQuery
                .Where(a => (userRole == "MedicalAnalyst" && a.LabEmail == Email) && a.IsAccepted == true)
                .Select(a => new AcceptAndCancelLabAppointment
                {
                    Id = a.Id,
                    Date = a.Date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    LabEmail = a.LabEmail,
                    PatientEmail = a.PatientEmail,
                    PatientSSN = a.PatientSSN,
                    PatientImage = a.PatientImage,
                    PhoneNumber = a.PhoneNumber,
                    IsAccepted = a.IsAccepted,
                    LabId = a.LabId,

                    Patientt = userRole == "MedicalAnalyst" ? new ApplicationUser
                    {
                        Id = a.PatientID,
                        FirstName = a.Patientt.FirstName,
                        LastName = a.Patientt.LastName,
                        PhoneNumber = a.Patientt.PhoneNumber,
                        Email = a.Patientt.Email,
                        SSN = a.Patientt.SSN,
                        Insurance_No = a.Patientt.Insurance_No,
                        BirthDate = a.Patientt.BirthDate,
                        Gender = a.Patientt.Gender,
                        ProfileImg = a.Patientt.ProfileImg,
                    } : null
                })
                .ToListAsync();

            return appointments;
        }

        public async Task<LabAppointment> GetAppointment(int id) =>
            await _context.LabAppointments
           .Include(d => d.Patient)
            .Include(p => p.Patientt)
            .Include(l => l.Lab)
            .FirstOrDefaultAsync(i => i.Id == id);


        public async Task<List<LabAppointment>> GetLabAppointmentByEmail(string Email, string userRole)
        {
            IQueryable<LabAppointment> appointmentsQuery = _context.LabAppointments;

            if (userRole == "MedicalAnalyst")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Patientt);
            }
            var appointments = await appointmentsQuery
                .Where(a => userRole == "MedicalAnalyst" && a.LabEmail == Email)
                .Select(a => new LabAppointment
                {
                    Id = a.Id,
                    date = a.date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    LabEmail = a.LabEmail,
                    PatientEmail = a.PatientEmail,
                    PatientSSN = a.PatientSSN,
                    PatientImage = a.PatientImage,
                    PhoneNumber = a.PhoneNumber,
                    LabId = a.LabId,
                    IsAccepted = a.IsAccepted,
                    PatientID = a.PatientID,

                    Patientt = userRole == "MedicalAnalyst" ? new ApplicationUser
                    {
                        Id = a.PatientID,
                        FirstName = a.Patientt.FirstName,
                        LastName = a.Patientt.LastName,
                        PhoneNumber = a.Patientt.PhoneNumber,
                        Email = a.Patientt.Email,
                        SSN = a.Patientt.SSN,
                        Insurance_No = a.Patientt.Insurance_No,
                        BirthDate = a.Patientt.BirthDate,
                        Gender = a.Patientt.Gender,
                        ProfileImg = a.Patientt.ProfileImg,
                    } : null
                })
                .ToListAsync();

            return appointments;
        }

        public async Task<IEnumerable<LabAppointment>> GetLabAppointmentByMedicalAnalyst(int id)
        {
            return await _context.LabAppointments
                .Where(d => d.LabId == id)
                .Include(p => p.Patient)
                .ToListAsync();
        }

        public async Task<List<LabAppointment>> GetLabAppointmentByUserId(string userId, string userRole)
        {
            IQueryable<LabAppointment> appointmentsQuery = _context.LabAppointments;

            if (userRole == "User")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Lab);
            }
            var appointments = await appointmentsQuery
                .Where(a => (userRole == "User" && a.PatientID == userId) && a.IsAccepted == false)
                .Select(a => new LabAppointment
                {
                    Id = a.Id,
                    date = a.date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    PatientSSN = a.PatientSSN,
                    PhoneNumber = a.PhoneNumber,
                    PatientEmail = a.PatientEmail,
                    PatientImage = a.PatientImage,
                    IsAccepted = a.IsAccepted,
                    PatientID = a.PatientID,
                    LabId = a.LabId,

                    Lab = userRole == "User" ? new Lab
                    {
                        Id = a.Id,
                        Name = a.Lab.Name,
                        Price = a.Lab.Price,
                        Location = a.Lab.Location,
                        Zone = a.Lab.Zone,
                        StartTime = a.Lab.StartTime,
                        EndTime = a.Lab.EndTime,
                        PhoneNumber = a.Lab.PhoneNumber,
                        LabImage = a.Lab.LabImage,
                        User = a.Lab.User,
                    } : null,
                })
                .ToListAsync();

            return appointments;
        }

        public async Task<IEnumerable<LabAppointment>> GetLabAppointments()
        {
            return await _context.LabAppointments
            .Include(p => p.Patient)
            .Include(d => d.Patientt)
             .Include(l => l.Lab)
            .ToListAsync();
        }

        public async Task<List<LabAppointment>> GetLabAppointmentsByPatientId(string userId, string userRole)
        {
            IQueryable<LabAppointment> appointmentsQuery = _context.LabAppointments;

            if (userRole == "User")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Lab);
            }
            var appointments = await appointmentsQuery
                .Where(a => userRole == "User" && a.PatientID == userId)
                .Select(a => new LabAppointment
                {
                    Id = a.Id,
                    date = a.date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    PatientSSN = a.PatientSSN,
                    PhoneNumber = a.PhoneNumber,
                    PatientEmail = a.PatientEmail,
                    PatientImage = a.PatientImage,
                    IsAccepted = a.IsAccepted,
                    PatientID = a.PatientID,
                    LabId = a.LabId,

                    Lab = userRole == "User" ? new Lab
                    {
                        Id = a.Id,
                        Name = a.Lab.Name,
                        Price = a.Lab.Price,
                        Location = a.Lab.Location,
                        Zone = a.Lab.Zone,
                        StartTime = a.Lab.StartTime,
                        EndTime = a.Lab.EndTime,
                        PhoneNumber = a.Lab.PhoneNumber,
                        LabImage = a.Lab.LabImage,
                        User = a.Lab.User,
                    } : null,
                })
                .ToListAsync();

            return appointments;
        }

        public async Task<IEnumerable<LabAppointment>> GetLabAppointmentWithPatient(long ssn)
        {
            return await _context.LabAppointments
                .Where(p => p.PatientSSN == ssn)
                .Include(p => p.Patient)
                .Include(d => d.Lab)
                .ToListAsync();
        }

        public async Task<List<LabAppointment>> GetWaitingLabAppointmentByEmail(string Email, string userRole)
        {
            IQueryable<LabAppointment> appointmentsQuery = _context.LabAppointments;

            if (userRole == "MedicalAnalyst")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Patientt);
            }
            var appointments = await appointmentsQuery
                .Where(a => (userRole == "MedicalAnalyst" && a.LabEmail == Email) && a.IsAccepted == false)
                .Select(a => new LabAppointment
                {
                    Id = a.Id,
                    date = a.date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    LabEmail = a.LabEmail,
                    PatientEmail = a.PatientEmail,
                    PatientSSN = a.PatientSSN,
                    PhoneNumber = a.PhoneNumber,
                    PatientImage = a.PatientImage,
                    LabId = a.LabId,
                    IsAccepted = a.IsAccepted,
                    PatientID = a.PatientID,

                    Patientt = userRole == "MedicalAnalyst" ? new ApplicationUser
                    {
                        Id = a.PatientID,
                        FirstName = a.Patientt.FirstName,
                        LastName = a.Patientt.LastName,
                        PhoneNumber = a.Patientt.PhoneNumber,
                        Email = a.Patientt.Email,
                        SSN = a.Patientt.SSN,
                        Insurance_No = a.Patientt.Insurance_No,
                        BirthDate = a.Patientt.BirthDate,
                        Gender = a.Patientt.Gender,
                        ProfileImg = a.Patientt.ProfileImg,
                    } : null
                })
                .ToListAsync();

            return appointments;
        }

        public LabAppointment Get_LabAppointment(int id) =>
              _context.LabAppointments
           .Include(d => d.Patient)
            .Include(p => p.Patientt)
            .Include(l => l.Lab)
            .FirstOrDefault(i => i.Id == id);

        public async Task<List<AcceptAndCancelLabAppointment>> SearchAcceptAndCancelLabAppointment(string userId, string userRole, DateTime? date)
        {
            var appointment = await GetAcceptAndCancelLabAppointment(userId, userRole);
            if ((date.HasValue && date != null) || date == DateTime.MinValue)
            {
                appointment = await _context.AcceptAndCancelLabAppointments.Where(x => x.Date.Year == date.Value.Year && x.Date.Month == date.Value.Month && x.Date.Day == date.Value.Day)
                    .Include(p => p.Patientt).Include(d => d.Lab).ToListAsync();
            }
            return appointment;
        }

        public async Task<List<AcceptAndCancelLabAppointment>> SearchAcceptLabAppointments(string Email, string userRole, DateTime? date, long? ssn)
        {
            long searchSSN = ssn ?? 0;
            var appointment = await GetAcceptLabAppointmentByMedical(Email, userRole);
            if ((date.HasValue && date != null) || date == DateTime.MinValue || (ssn != null && ssn != 0))
            {
                appointment = await _context.AcceptAndCancelLabAppointments.Where(x => (date.HasValue && x.Date.Year == date.Value.Year && x.Date.Month == date.Value.Month && x.Date.Day == date.Value.Day && x.IsAccepted == true)
                || (x.PatientSSN == searchSSN && x.IsAccepted == true)).Include(p => p.Patientt).ToListAsync();
            }
            return appointment;
        }
    }
}
