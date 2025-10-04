using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToLogin);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO objToRegister);
    }
}
