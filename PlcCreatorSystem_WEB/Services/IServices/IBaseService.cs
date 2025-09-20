using PlcCreatorSystem_WEB.Models;

namespace PlcCreatorSystem_WEB.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
