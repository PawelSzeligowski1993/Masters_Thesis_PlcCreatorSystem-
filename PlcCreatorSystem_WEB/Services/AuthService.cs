using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Services.IServices;

namespace PlcCreatorSystem_WEB.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string? authorUrl;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            authorUrl = configuration.GetValue<string>("ServiceUrls:Creator_API");
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO objToLogin)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = objToLogin,
                Url = authorUrl + "/api/Users_API/login"
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDTO objToRegister)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = objToRegister,
                Url = authorUrl + "/api/Users_API/register"
            });
        }
    }
}
