using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Services.IServices
{
    public interface IHmiService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(HmiCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(HmiUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
