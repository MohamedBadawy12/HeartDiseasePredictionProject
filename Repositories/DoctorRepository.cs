using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task Add(Doctor doctor) => await _context.Doctors.AddAsync(doctor);

        public async Task<IEnumerable<Doctor>> FilterDoctors(string search)
        {
            var doctors = await GetDoctors();
            if (!string.IsNullOrEmpty(search))
            {
                doctors = await _context.Doctors.
                Where(x => x.Name.Contains(search) || x.User.FirstName.Contains(search) || x.User.LastName.Contains(search)).ToListAsync();
            }
            return doctors;
        }
        public async Task<IEnumerable<Doctor>> GetAvailableDoctors()
        {
            return await _context.Doctors
                .Where(a => a.IsAvailable == true)
                .Include(u => u.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetDoctors()
        {
            return await _context.Doctors
                .Include(d => d.User)
                .ToListAsync();
        }

        public async Task<Doctor> GetDoctor(int id)
        {
            return await _context.Doctors
               .Include(d => d.User)
               .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor> GetProfile(string userId)
        {
            return await _context.Doctors.Include(u => u.User).FirstOrDefaultAsync(d => d.UserId == userId);
            //.Include(d => d.medicalTests)
        }

        public Doctor Get_Doctor(int id)
        {
            return _context.Doctors
               .Include(d => d.User)
               .Include(d => d.Appointments)
               .Include(d => d.Patients)
               .Include(d => d.prescriptions)
               .FirstOrDefault(d => d.Id == id);
        }

        public void Delete(Doctor doctor) => _context.Doctors.Remove(doctor);

        public Doctor FindDoctor(int id) =>
             _context.Doctors.Find(id);

        public bool DeleteDoctor(int id)
        {
            var isDeleted = false;

            var doctor = _context.Doctors
               .Include(d => d.User)
               .Include(d => d.Appointments)
               .Include(d => d.Patients)
               .Include(d => d.prescriptions)
               .FirstOrDefault(d => d.Id == id);

            if (doctor is null)
                return isDeleted;

            _context.Remove(doctor);
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;
            }

            return isDeleted;
        }
    }
}