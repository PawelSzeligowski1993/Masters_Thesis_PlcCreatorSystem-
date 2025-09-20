using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Services.IServices
{
    public interface IHmiService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(HmiCreateDTO dto);
        Task<T> UpdateAsync<T>(HmiUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
