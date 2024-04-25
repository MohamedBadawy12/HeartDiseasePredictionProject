using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<MedicalAnalyst> MedicalAnalysts { get; set; }
        public virtual DbSet<MedicalTest> MedicalTests { get; set; }
        //public virtual DbSet<TestsAndPrediction> TestsAndPredictions { get; set; }
        //public virtual DbSet<Prediction> Predictions { get; set; }
        public virtual DbSet<Reciptionist> Reciptionists { get; set; }
        public virtual DbSet<Lab> Labs { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<AcceptAndCancelAppointment> AcceptAndCancelAppointments { get; set; }
        public virtual DbSet<AcceptAndCancelLabAppointment> AcceptAndCancelLabAppointments { get; set; }
        public virtual DbSet<LabAppointment> LabAppointments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Patient>().HasIndex(x => new { x.Insurance_No }).IsUnique();

            modelBuilder.Entity<Appointment>()
               .HasOne(a => a.Patient)
               .WithMany(p => p.Appointments)
               .HasForeignKey(a => a.PatientSSN)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescription>()
               .HasOne(a => a.Patient)
               .WithMany(p => p.Prescriptions)
               .HasForeignKey(a => a.PatientSSN)
               .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<MedicalTest>()
            //.HasOne(p => p.Prediction)
            //.WithOne(mt => mt.MedicalTest)
            //.HasForeignKey<Prediction>(p => p.MedicalTestId);
        }
    }
}
