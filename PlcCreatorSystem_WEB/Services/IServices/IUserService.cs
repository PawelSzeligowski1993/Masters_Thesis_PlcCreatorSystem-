using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Services.IServices
{
    public interface IUserService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> UpdateAsync<T>(UserUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
