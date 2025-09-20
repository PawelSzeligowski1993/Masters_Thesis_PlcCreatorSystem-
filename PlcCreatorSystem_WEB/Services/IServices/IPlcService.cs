using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Services.IServices
{
    public interface IPlcService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(PlcCreateDTO dto);
        Task<T> UpdateAsync<T>(PlcUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
