using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.ViewModel;

namespace Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Appointment>> GetAppointments()
        {
            return await _context.Appointments
                .Include(p => p.Patient)
                .Include(d => d.Doctor)
                .ToListAsync();
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentWithPatient(long ssn)
        {
            return await _context.Appointments
                .Where(p => p.PatientSSN == ssn)
                .Include(p => p.Patient)
                .Include(d => d.Doctor)
                .ToListAsync();
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentByDoctor(int id)
        {
            //return (from a in _context.Appointments where a.DoctorId == id select a).AsEnumerable();

            return await _context.Appointments
                .Where(d => d.DoctorId == id)
                .Include(p => p.Patient)
                .ToListAsync();
        }
        public async Task<int> CountAppointments(long ssn)
        {
            return await _context.Appointments.CountAsync(a => a.PatientSSN == ssn);
        }


        public async Task<Appointment> GetAppointment(int id) =>
            await _context.Appointments
            .Include(d => d.Doctor)
            .Include(p => p.Patientt)
            .Include(P => P.Patient)
            .FirstOrDefaultAsync(i => i.Id == id);

        public async Task<AcceptAndCancelAppointment> GetAcceptAppointment(int id) =>
            await _context.AcceptAndCancelAppointments
            .Include(d => d.Doctor)
            .Include(p => p.Patientt)
            .Include(P => P.Patient)
            .FirstOrDefaultAsync(i => i.Id == id);


        public async Task AddAsync(Appointment appointment) =>
            await _context.Appointments.AddAsync(appointment);

        public async Task AddAppointmentAsync(AppointmentViewModel model)
        {
            var appointment = new Appointment()
            {
                PatientID = model.PatientID,
                DoctorEmail = model.DoctorEmail,
                PatientEmail = model.PatientEmail,
                PatientSSN = model.PatientSSN,
                DoctorId = model.DoctorId,
                date = model.date,
                Time = model.Time,
            };
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientId(string userId, string userRole)
        {
            IQueryable<Appointment> appointmentsQuery = _context.Appointments;

            if (userRole == "User")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Doctor);
            }
            var appointments = await appointmentsQuery
                .Where(a => (userRole == "User" && a.PatientID == userId) && a.IsAccepted == false)
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    date = a.date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    DoctorEmail = a.DoctorEmail,
                    PatientSSN = a.PatientSSN,
                    PhoneNumber = a.PhoneNumber,
                    PatientEmail = a.PatientEmail,
                    PatientImage = a.PatientImage,
                    IsAccepted = a.IsAccepted,
                    PatientID = a.PatientID,
                    DoctorId = a.DoctorId,

                    Doctor = userRole == "User" ? new Doctor
                    {
                        Id = a.Id,
                        Name = a.Doctor.Name,
                        Price = a.Doctor.Price,
                        Location = a.Doctor.Location,
                        Zone = a.Doctor.Zone,
                        IsAvailable = a.Doctor.IsAvailable,
                        User = a.Doctor.User,
                    } : null,
                })
                .ToListAsync();

            return appointments;
        }


        public async Task<List<Appointment>> GetAppointmentByUserId(string userId, string userRole)
        {
            IQueryable<Appointment> appointmentsQuery = _context.Appointments;

            if (userRole == "User")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Doctor);
            }
            var appointments = await appointmentsQuery
                .Where(a => (userRole == "User" && a.PatientID == userId))
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    date = a.date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    DoctorEmail = a.DoctorEmail,
                    PatientSSN = a.PatientSSN,
                    PatientImage = a.PatientImage,
                    PhoneNumber = a.PhoneNumber,
                    PatientEmail = a.PatientEmail,
                    IsAccepted = a.IsAccepted,
                    PatientID = a.PatientID,
                    DoctorId = a.DoctorId,

                    Doctor = userRole == "User" ? new Doctor
                    {
                        Id = a.Id,
                        Name = a.Doctor.Name,
                        Price = a.Doctor.Price,
                        Location = a.Doctor.Location,
                        Zone = a.Doctor.Zone,
                        IsAvailable = a.Doctor.IsAvailable,
                        User = a.Doctor.User,
                    } : null,
                })
                .ToListAsync();

            return appointments;
        }


        public async Task<List<Appointment>> GetAppointmentByEmail(string Email, string userRole)
        {
            IQueryable<Appointment> appointmentsQuery = _context.Appointments;

            if (userRole == "Doctor")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Patientt);
            }
            var appointments = await appointmentsQuery
                .Where(a => userRole == "Doctor" && a.DoctorEmail == Email)
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    date = a.date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    DoctorEmail = a.DoctorEmail,
                    PatientEmail = a.PatientEmail,
                    PatientImage = a.PatientImage,
                    PatientSSN = a.PatientSSN,
                    PhoneNumber = a.PhoneNumber,
                    DoctorId = a.DoctorId,
                    IsAccepted = a.IsAccepted,
                    PatientID = a.PatientID,

                    Patientt = userRole == "Doctor" ? new ApplicationUser
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

        public async Task<List<Appointment>> GetWaitingAppointmentByEmail(string Email, string userRole)
        {
            IQueryable<Appointment> appointmentsQuery = _context.Appointments;

            if (userRole == "Doctor")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Patientt);
            }
            var appointments = await appointmentsQuery
                .Where(a => (userRole == "Doctor" && a.DoctorEmail == Email) && a.IsAccepted == false)
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    date = a.date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    DoctorEmail = a.DoctorEmail,
                    PatientEmail = a.PatientEmail,
                    PatientSSN = a.PatientSSN,
                    PatientImage = a.PatientImage,
                    PhoneNumber = a.PhoneNumber,
                    DoctorId = a.DoctorId,
                    IsAccepted = a.IsAccepted,
                    PatientID = a.PatientID,

                    Patientt = userRole == "Doctor" ? new ApplicationUser
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

        public void Cancel(Appointment appointment) =>
            _context.Appointments.Remove(appointment);

        public Appointment Get_Appointment(int id) =>
             _context.Appointments
            .Include(d => d.Doctor)
            .Include(p => p.Patient)
            .Include(P => P.Patientt)
            .FirstOrDefault(i => i.Id == id);

        public async Task AddMessageAsync(Message message) =>
            await _context.Messages.AddAsync(message);

        public async Task<List<Message>> GetMessageByUserId(string userId, string userRole)
        {
            var messages = await _context.Messages
                 .Include(p => p.Doctor)
                 .ToListAsync();
            if (userRole == "Doctor")
            {
                messages = messages.Where(n => n.DoctorId == userId).ToList();
            }
            return messages;
        }
        public async Task<List<Message>> GetMessageByEmail(string Email, string userRole)
        {
            var messages = await _context.Messages
                 .Include(p => p.Doctor)
                 .ToListAsync();
            if (userRole == "User")
            {
                messages = messages.Where(n => n.PatientEmail == Email).ToList();
            }
            return messages;
        }

        public bool Canceled(int id)
        {
            var isDeleted = false;

            var appointment = _context.Appointments
            .Include(d => d.Doctor)
            .Include(p => p.Patient)
            .Include(P => P.Patientt)
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

        public async Task<List<AcceptAndCancelAppointment>> GetAcceptAndCancelAppointment(string userId, string userRole)
        {
            IQueryable<AcceptAndCancelAppointment> appointmentsQuery = _context.AcceptAndCancelAppointments;

            if (userRole == "User")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Doctor);
            }
            var appointments = await appointmentsQuery
                .Where(a => (userRole == "User" && a.PatientID == userId))
                .Select(a => new AcceptAndCancelAppointment
                {
                    Id = a.Id,
                    Date = a.Date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    DoctorEmail = a.DoctorEmail,
                    PatientEmail = a.PatientEmail,
                    PatientSSN = a.PatientSSN,
                    PatientImage = a.PatientImage,
                    PhoneNumber = a.PhoneNumber,
                    IsAccepted = a.IsAccepted,
                    DoctorId = a.DoctorId,

                    Doctor = userRole == "User" ? new Doctor
                    {
                        Id = a.Id,
                        Name = a.Doctor.Name,
                        Price = a.Doctor.Price,
                        Location = a.Doctor.Location,
                        Zone = a.Doctor.Zone,
                        IsAvailable = a.Doctor.IsAvailable,
                        User = a.Doctor.User,
                    } : null,
                })
                .ToListAsync();

            return appointments;
        }

        public async Task AddAcceptOrCancelAsync(AcceptAndCancelAppointment appointment) =>
             await _context.AcceptAndCancelAppointments.AddAsync(appointment);

        public async Task<List<AcceptAndCancelAppointment>> GetAcceptAppointmentByDoctor(string Email, string userRole)
        {
            IQueryable<AcceptAndCancelAppointment> appointmentsQuery = _context.AcceptAndCancelAppointments;

            if (userRole == "Doctor")
            {
                appointmentsQuery = appointmentsQuery.Include(a => a.Patientt);
            }
            var appointments = await appointmentsQuery
                .Where(a => (userRole == "Doctor" && a.DoctorEmail == Email) && a.IsAccepted == true)
                .Select(a => new AcceptAndCancelAppointment
                {
                    Id = a.Id,
                    Date = a.Date,
                    Time = a.Time,
                    PateintName = a.PateintName,
                    DoctorEmail = a.DoctorEmail,
                    PatientEmail = a.PatientEmail,
                    PatientSSN = a.PatientSSN,
                    PatientImage = a.PatientImage,
                    PhoneNumber = a.PhoneNumber,
                    IsAccepted = a.IsAccepted,
                    DoctorId = a.DoctorId,

                    Patientt = userRole == "Doctor" ? new ApplicationUser
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
        public async Task<List<Message>> SearchMessages(string Email, string userRole, DateTime? date)
        {
            var message = await GetMessageByEmail(Email, userRole);
            if ((date.HasValue && date != null) || date == DateTime.MinValue)
            {
                message = await _context.Messages.Where(x => x.Date.Year == date.Value.Year && x.Date.Month == date.Value.Month
                && x.Date.Day == date.Value.Day).ToListAsync();
            }
            return message;
        }

        public async Task<List<AcceptAndCancelAppointment>> SearchAcceptAndCancelAppointment(string userId, string userRole, DateTime? date)
        {
            var appointment = await GetAcceptAndCancelAppointment(userId, userRole);
            if ((date.HasValue && date != null) || date == DateTime.MinValue)
            {
                appointment = await _context.AcceptAndCancelAppointments.Where(x => x.Date.Year == date.Value.Year && x.Date.Month == date.Value.Month && x.Date.Day == date.Value.Day)
                    .Include(p => p.Patientt).Include(d => d.Doctor).ToListAsync();
            }
            return appointment;
        }

        public async Task<List<AcceptAndCancelAppointment>> SearchAcceptAppointments(string Email, string userRole, DateTime? date, long? ssn)
        {
            long searchSSN = ssn ?? 0;
            var appointment = await GetAcceptAppointmentByDoctor(Email, userRole);
            if ((date.HasValue && date != null) || date == DateTime.MinValue || (ssn != null && ssn != 0))
            {
                appointment = await _context.AcceptAndCancelAppointments.Where(x => (date.HasValue && x.Date.Year == date.Value.Year && x.Date.Month == date.Value.Month && x.Date.Day == date.Value.Day && x.IsAccepted == true)
                || (x.PatientSSN == searchSSN && x.IsAccepted == true)).Include(p => p.Patientt).ToListAsync();
            }
            return appointment;
        }
    }
}
