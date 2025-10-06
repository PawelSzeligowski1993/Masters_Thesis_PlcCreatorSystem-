using PlcCreatorSystem_API.Models;
using PlcCreatorSystem_API.Models.Dto;

namespace PlcCreatorSystem_API.Repository.IRepository
{
    public interface IUserRepository : IRepository<LocalUser>
    {
        bool IsUniqueUsers(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegisterationRequestDTO registrationRequestDTO);
        Task<LocalUser> UpdateAsync(LocalUser entity);
    }
}
