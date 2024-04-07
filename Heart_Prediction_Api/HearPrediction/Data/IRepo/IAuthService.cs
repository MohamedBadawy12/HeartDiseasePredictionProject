using Database.Entities;
using HearPrediction.Api.DTO;
using System.Threading.Tasks;

namespace Services.Interfaces
{
	public interface IAuthService
	{
		Task<AuthModel> RegisterDoctorAsync(RegisterDoctorDTO registerDoctorDTO);
		Task<AuthModel> RegisterUserAsync(RegisterUserDTO registerUserDTO);
		Task<AuthModel> RegisterMedicalAnalystAsync(RegisterMedicalAnalystDTO registerMedicalAnalystDTO);
		Task<AuthModel> RegisterReceptionistAsync(RegisterReciptionistDTO registerReciptionistDTO);
		Task<AuthModel> ConfirmationOfEmail(ConfirmEmailDto confirm);
		Task<AuthModel> LoginTokenAsync(TokenRequestModel model);
		Task<AuthModel> LoginWithOTP(string code, string Email);
		//Task<AuthModel> LogoutAsync();
		Task<AuthModel> RefreshTokenAsync(string token);
		Task<bool> RevokeTokenAsync(string token);
	}
}
