using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Services.IServices
{
    public interface IPlcService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(PlcCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(PlcUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
