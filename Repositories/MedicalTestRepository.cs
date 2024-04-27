using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class MedicalTestRepository : IMedicalTestRepository
    {
        private readonly AppDbContext _context;
        public MedicalTestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MedicalTest medicalTest) =>
            await _context.MedicalTests.AddAsync(medicalTest);


        public async Task<List<MedicalTest>> GetMedicalTestsByMedicalId(string userId, string userRole)
        {
            IQueryable<MedicalTest> MedicalTestsQuery = _context.MedicalTests;

            if (userRole == "MedicalAnalyst")
            {
                MedicalTestsQuery = MedicalTestsQuery.Include(a => a.Patient);
            }
            var medicalTests = await MedicalTestsQuery
                 .Where(a => (userRole == "MedicalAnalyst" && a.UserId == userId))
                .Select(a => new MedicalTest
                {
                    Id = a.Id,
                    PatientEmail = a.PatientEmail,
                    PatientName = a.PatientName,
                    PatientSSN = a.PatientSSN,
                    Date = a.Date,
                    MedicalAnalystName = a.MedicalAnalystName,
                    LabEmail = a.LabEmail,
                    LabId = a.LabId,

                    Patient = userRole == "MedicalAnalyst" ? new Patient
                    {
                        SSN = a.PatientSSN,
                        Insurance_No = a.Patient.Insurance_No,
                        User = a.Patient.User,
                    } : null
                })
                .ToListAsync();

            return medicalTests;
        }

        public async Task<List<MedicalTest>> GetMedicalTestsByUserId(string userId, string userRole)
        {
            IQueryable<MedicalTest> MedicalTestsQuery = _context.MedicalTests;

            if (userRole == "MedicalAnalyst")
            {
                MedicalTestsQuery = MedicalTestsQuery.Include(a => a.Patient);
            }
            var medicalTests = await MedicalTestsQuery
                 .Where(a => (userRole == "MedicalAnalyst" && a.UserId == userId))
                .Select(a => new MedicalTest
                {
                    Id = a.Id,
                    Gender = a.Gender,
                    GlucoseLevel = a.GlucoseLevel,
                    Diabetes = a.Diabetes,
                    DiastolicBloodPressure = a.DiastolicBloodPressure,
                    BMI = a.BMI,
                    Age = a.Age,
                    BloodPressureMedicine = a.BloodPressureMedicine,
                    CholesterolLevel = a.CholesterolLevel,
                    NumberOfCigarettes = a.NumberOfCigarettes,
                    Smoking = a.Smoking,
                    SystolicBloodPressure = a.SystolicBloodPressure,
                    Prevalenthypertension = a.Prevalenthypertension,
                    PrevalentStroke = a.PrevalentStroke,
                    HeartRate = a.HeartRate,
                    PatientEmail = a.PatientEmail,
                    PatientName = a.PatientName,
                    PatientSSN = a.PatientSSN,
                    Date = a.Date,
                    MedicalAnalystName = a.MedicalAnalystName,
                    LabEmail = a.LabEmail,
                    LabId = a.LabId,
                    Prediction = a.Prediction,
                    Probability = a.Probability,

                    Patient = userRole == "MedicalAnalyst" ? new Patient
                    {
                        SSN = a.PatientSSN,
                        Insurance_No = a.Patient.Insurance_No,
                        User = a.Patient.User,
                    } : null
                })
                .ToListAsync();

            return medicalTests;
        }

        public async Task<List<MedicalTest>> GetMedicalTestsByPatientEmail(string Email, string userRole)
        {
            IQueryable<MedicalTest> MedicalTestsQuery = _context.MedicalTests;

            if (userRole == "User")
            {
                MedicalTestsQuery = MedicalTestsQuery.Include(a => a.Patient);
            }
            var medicalTests = await MedicalTestsQuery
                 .Where(a => (userRole == "User" && a.PatientEmail == Email))
                .Select(a => new MedicalTest
                {
                    Id = a.Id,
                    Date = a.Date,
                    PatientEmail = a.PatientEmail,
                    PatientName = a.PatientName,
                    PatientSSN = a.PatientSSN,
                    MedicalAnalystName = a.MedicalAnalystName,
                    LabEmail = a.LabEmail,
                    LabId = a.LabId,

                    Labb = userRole == "User" ? new ApplicationUser
                    {
                        Id = a.UserId,
                        Name = a.Labb.Name,
                        PhoneNumber = a.Labb.PhoneNumber,
                        Location = a.Labb.Location,
                        Price = a.Labb.Price,
                        Email = a.Labb.Email,
                        ProfileImg = a.Labb.ProfileImg,
                    } : null,
                    Lab = userRole == "User" ? new Lab
                    {
                        Id = a.Id,
                        Location = a.Lab.Location,
                        Price = a.Lab.Price,
                        PhoneNumber = a.Lab.PhoneNumber,
                        Name = a.Lab.Name,
                        LabImage = a.Lab.LabImage,
                        User = a.Labb,
                    } : null,
                })
                .ToListAsync();

            return medicalTests;
        }

        public async Task<List<MedicalTest>> GetMedicalTestsByEmail(string Email, string userRole)
        {
            IQueryable<MedicalTest> MedicalTestsQuery = _context.MedicalTests;

            if (userRole == "User")
            {
                MedicalTestsQuery = MedicalTestsQuery.Include(a => a.Patient);
            }
            var medicalTests = await MedicalTestsQuery
                 .Where(a => (userRole == "User" && a.PatientEmail == Email))
                .Select(a => new MedicalTest
                {
                    Id = a.Id,
                    Gender = a.Gender,
                    GlucoseLevel = a.GlucoseLevel,
                    Diabetes = a.Diabetes,
                    DiastolicBloodPressure = a.DiastolicBloodPressure,
                    BMI = a.BMI,
                    Age = a.Age,
                    BloodPressureMedicine = a.BloodPressureMedicine,
                    CholesterolLevel = a.CholesterolLevel,
                    NumberOfCigarettes = a.NumberOfCigarettes,
                    Smoking = a.Smoking,
                    SystolicBloodPressure = a.SystolicBloodPressure,
                    Prevalenthypertension = a.Prevalenthypertension,
                    PrevalentStroke = a.PrevalentStroke,
                    HeartRate = a.HeartRate,
                    Date = a.Date,
                    PatientEmail = a.PatientEmail,
                    PatientName = a.PatientName,
                    PatientSSN = a.PatientSSN,
                    MedicalAnalystName = a.MedicalAnalystName,
                    LabEmail = a.LabEmail,
                    LabId = a.LabId,
                    Prediction = a.Prediction,
                    Probability = a.Probability,

                    Labb = userRole == "User" ? new ApplicationUser
                    {
                        Id = a.UserId,
                        Name = a.Labb.Name,
                        PhoneNumber = a.Labb.PhoneNumber,
                        Location = a.Labb.Location,
                        Price = a.Labb.Price,
                        Email = a.Labb.Email,
                        ProfileImg = a.Labb.ProfileImg,
                    } : null,
                    Lab = userRole == "User" ? new Lab
                    {
                        Id = a.Id,
                        Location = a.Lab.Location,
                        Price = a.Lab.Price,
                        PhoneNumber = a.Lab.PhoneNumber,
                        Name = a.Lab.Name,
                        LabImage = a.Lab.LabImage,
                        User = a.Labb,
                    } : null,
                })
                .ToListAsync();

            return medicalTests;
        }

        public async Task<List<MedicalTest>> SearchMedicalTestsByUserId(string userId, string userRole, DateTime? date, long? ssn)
        {
            long searchSSN = ssn ?? 0;
            var medicalTests = await GetMedicalTestsByUserId(userId, userRole);
            if ((date.HasValue && date != null) || date == DateTime.MinValue || (ssn != null && ssn != 0))
            {
                medicalTests = await _context.MedicalTests.Where(x => (date.HasValue && x.Date.Year == date.Value.Year && x.Date.Month == date.Value.Month && x.Date.Day == date.Value.Day)
                || (x.PatientSSN == searchSSN)).Include(p => p.Patient).ToListAsync();
            }
            return medicalTests;
        }

        public async Task<List<MedicalTest>> SearchMedicalTestsByEmail(string Email, string userRole, DateTime? date)
        {
            var medicalTests = await GetMedicalTestsByEmail(Email, userRole);
            if ((date.HasValue && date != null) || date == DateTime.MinValue)
            {
                medicalTests = await _context.MedicalTests.Where(x => (date.HasValue && x.Date.Year == date.Value.Year && x.Date.Month == date.Value.Month && x.Date.Day == date.Value.Day && userRole == "User" && x.PatientEmail == Email))
                    .Include(d => d.Lab).Include(d => d.Labb).ToListAsync();
            }
            return medicalTests;
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var medicalTest = _context.MedicalTests
            .Include(p => p.Lab)
            .Include(p => p.Labb)
            .Include(D => D.Patient)
            //.Include(D => D.Prediction)
            .FirstOrDefault(i => i.Id == id);

            if (medicalTest is null)
                return isDeleted;

            _context.Remove(medicalTest);
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;
            }

            return isDeleted;
        }

        public async Task<MedicalTest> GetMedicalTest(int id) =>
            await _context.MedicalTests
             .Include(p => p.Lab)
            .Include(p => p.Labb)
            .Include(D => D.Patient)
            .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<IEnumerable<MedicalTest>> GetMedicalTests() =>
            await _context.MedicalTests
            .Include(p => p.Lab)
            .Include(p => p.Labb)
            .Include(D => D.Patient)
            .ToListAsync();


        public MedicalTest Get_MedicalTest(int id) =>
              _context.MedicalTests
             .Include(p => p.Lab)
            .Include(p => p.Labb)
            .Include(D => D.Patient)
            .FirstOrDefault(m => m.Id == id);

        public void Remove(MedicalTest medicalTest) =>
            _context.MedicalTests.Remove(medicalTest);


    }
}
