using Database.Entities;
using Repositories.ViewModel;

namespace Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task AddAppointmentAsync(AppointmentViewModel model);
        Task<List<Appointment>> GetAppointmentByUserId(string userId, string userRole);
        Task<List<Appointment>> GetAppointmentsByPatientId(string userId, string userRole);
        Task<List<Appointment>> GetAppointmentByEmail(string Email, string userRole);
        Task<List<Appointment>> GetWaitingAppointmentByEmail(string Email, string userRole);
        Task<List<AcceptAndCancelAppointment>> GetAcceptAndCancelAppointment(string userId, string userRole);
        Task<List<AcceptAndCancelAppointment>> GetAcceptAppointmentByDoctor(string Email, string userRole);
        Task<List<Message>> GetMessageByUserId(string userId, string userRole);
        Task<List<Message>> GetMessageByEmail(string Email, string userRole);
        Task<IEnumerable<Appointment>> GetAppointments();
        Task<IEnumerable<Appointment>> GetAppointmentWithPatient(long ssn);
        Task<IEnumerable<Appointment>> GetAppointmentByDoctor(int id);
        Task<int> CountAppointments(long ssn);
        Task<Appointment> GetAppointment(int id);
        Task<List<Message>> SearchMessages(string Email, string userRole, DateTime? date);
        Task<List<AcceptAndCancelAppointment>> SearchAcceptAndCancelAppointment(string userId, string userRole, DateTime? date);
        Task<List<AcceptAndCancelAppointment>> SearchAcceptAppointments(string Email, string userRole, DateTime? date, long? ssn);
        Task<AcceptAndCancelAppointment> GetAcceptAppointment(int id);
        Appointment Get_Appointment(int id);
        Task AddAsync(Appointment appointment);
        Task AddAcceptOrCancelAsync(AcceptAndCancelAppointment appointment);
        Task AddMessageAsync(Message message);
        void Cancel(Appointment appointment);
        bool Canceled(int id);
    }
}
