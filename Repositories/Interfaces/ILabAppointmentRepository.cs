using Database.Entities;

namespace Repositories.Interfaces
{
    public interface ILabAppointmentRepository
    {
        Task<List<LabAppointment>> GetLabAppointmentByUserId(string userId, string userRole);
        Task<List<LabAppointment>> GetLabAppointmentsByPatientId(string userId, string userRole);
        Task<List<LabAppointment>> GetLabAppointmentByEmail(string Email, string userRole);
        Task<List<LabAppointment>> GetWaitingLabAppointmentByEmail(string Email, string userRole);
        Task<List<AcceptAndCancelLabAppointment>> GetAcceptAndCancelLabAppointment(string userId, string userRole);
        Task<List<AcceptAndCancelLabAppointment>> GetAcceptLabAppointmentByMedical(string Email, string userRole);
        Task<IEnumerable<LabAppointment>> GetLabAppointments();
        Task<IEnumerable<LabAppointment>> GetLabAppointmentWithPatient(long ssn);
        Task<IEnumerable<LabAppointment>> GetLabAppointmentByMedicalAnalyst(int id);
        Task<List<AcceptAndCancelLabAppointment>> SearchAcceptAndCancelLabAppointment(string userId, string userRole, DateTime? date);
        Task<List<AcceptAndCancelLabAppointment>> SearchAcceptLabAppointments(string Email, string userRole, DateTime? date, long? ssn);
        Task<AcceptAndCancelLabAppointment> GetAcceptAppointment(int id);
        Task<LabAppointment> GetAppointment(int id);
        LabAppointment Get_LabAppointment(int id);
        Task AddAsync(LabAppointment appointment);
        Task AddAcceptOrCancelAsync(AcceptAndCancelLabAppointment appointment);
        //void Cancel(LabAppointment appointment);
        bool Canceled(int id);
    }
}
