using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Services.IServices
{
    public interface IProjectService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(ProjectCreateDTO dto);
        Task<T> UpdateAsync<T>(ProjectUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
