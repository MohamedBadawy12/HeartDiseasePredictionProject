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


        public async Task<List<MedicalTest>> GetMedicalTestsByUserId(string userId, string userRole)
        {
            IQueryable<MedicalTest> MedicalTestsQuery = _context.MedicalTests;

            if (userRole == "MedicalAnalyst")
            {
                MedicalTestsQuery = MedicalTestsQuery.Include(a => a.patient);
            }
            var medicalTests = await MedicalTestsQuery
                 .Where(a => (userRole == "MedicalAnalyst" && a.UserId == userId))
                .Select(a => new MedicalTest
                {
                    Id = a.Id,
                    date = a.date,
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
                    PatientEmail = a.PatientEmail,
                    PatientSSN = a.PatientSSN,
                    MedicalAnalystEmail = a.MedicalAnalystEmail,
                    MedicalAnalystId = a.MedicalAnalystId,

                    Prediction = userRole == "MedicalAnalyst" ? new Prediction
                    {
                        Id = a.Id,
                        prediction = a.Prediction.prediction,
                        Probability = a.Prediction.Probability,
                        Score = a.Prediction.Score,
                    } : null,
                    patient = userRole == "MedicalAnalyst" ? new Patient
                    {
                        SSN = a.PatientSSN,
                        Insurance_No = a.patient.Insurance_No,
                        User = a.patient.User,
                    } : null
                })
                .ToListAsync();

            return medicalTests;
        }
        public async Task<List<MedicalTest>> GetMedicalTestsByEmail(string Email, string userRole)
        {
            IQueryable<MedicalTest> MedicalTestsQuery = _context.MedicalTests;

            if (userRole == "User")
            {
                MedicalTestsQuery = MedicalTestsQuery.Include(a => a.patient);
            }
            var medicalTests = await MedicalTestsQuery
                 .Where(a => (userRole == "User" && a.PatientEmail == Email))
                .Select(a => new MedicalTest
                {
                    Id = a.Id,
                    date = a.date,
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
                    PatientEmail = a.PatientEmail,
                    PatientSSN = a.PatientSSN,
                    MedicalAnalystEmail = a.MedicalAnalystEmail,
                    MedicalAnalystId = a.MedicalAnalystId,

                    Prediction = userRole == "User" ? new Prediction
                    {
                        Id = a.Id,
                        prediction = a.Prediction.prediction,
                        Probability = a.Prediction.Probability,
                        Score = a.Prediction.Score,
                    } : null,
                    MedicalAnalayst = userRole == "User" ? new ApplicationUser
                    {
                        Id = a.UserId,
                        FirstName = a.MedicalAnalayst.FirstName,
                        LastName = a.MedicalAnalayst.LastName,
                        PhoneNumber = a.MedicalAnalayst.PhoneNumber,
                        Email = a.MedicalAnalayst.Email,
                        BirthDate = a.MedicalAnalayst.BirthDate,
                        Gender = a.MedicalAnalayst.Gender,
                    } : null,
                    MedicalAnalystt = userRole == "User" ? new MedicalAnalyst
                    {
                        Id = a.Id,
                        LabId = a.MedicalAnalystt.LabId,
                        Lab = a.MedicalAnalystt.Lab,
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
                medicalTests = await _context.MedicalTests.Where(x => (date.HasValue && x.date.Year == date.Value.Year && x.date.Month == date.Value.Month && x.date.Day == date.Value.Day)
                || (x.PatientSSN == searchSSN)).Include(p => p.patient).Include(p => p.Prediction).ToListAsync();
            }
            return medicalTests;
        }

        public async Task<List<MedicalTest>> SearchMedicalTestsByEmail(string Email, string userRole, DateTime? date)
        {
            var medicalTests = await GetMedicalTestsByEmail(Email, userRole);
            if ((date.HasValue && date != null) || date == DateTime.MinValue)
            {
                medicalTests = await _context.MedicalTests.Where(x => (date.HasValue && x.date.Year == date.Value.Year && x.date.Month == date.Value.Month && x.date.Day == date.Value.Day && userRole == "User" && x.PatientEmail == Email))
                    .Include(d => d.MedicalAnalayst).Include(d => d.MedicalAnalystt).Include(p => p.Prediction).ToListAsync();
            }
            return medicalTests;
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var medicalTest = _context.MedicalTests
            .Include(p => p.MedicalAnalayst)
            .Include(p => p.MedicalAnalystt)
            .Include(D => D.patient)
            .Include(D => D.Prediction)
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
            .Include(x => x.MedicalAnalayst)
            .Include(x => x.MedicalAnalystt)
            .Include(x => x.patient)
            .Include(D => D.Prediction)
            .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<IEnumerable<MedicalTest>> GetMedicalTests() =>
            await _context.MedicalTests
            .Include(x => x.MedicalAnalayst)
            .Include(x => x.MedicalAnalystt)
            .Include(x => x.patient)
             .Include(D => D.Prediction)
            .ToListAsync();


        public MedicalTest Get_MedicalTest(int id) =>
              _context.MedicalTests
            .Include(x => x.MedicalAnalayst)
            .Include(x => x.MedicalAnalystt)
            .Include(x => x.patient)
             .Include(D => D.Prediction)
            .FirstOrDefault(m => m.Id == id);

        public void Remove(MedicalTest medicalTest) =>
            _context.MedicalTests.Remove(medicalTest);


    }
}
