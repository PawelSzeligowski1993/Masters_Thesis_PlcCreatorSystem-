using PlcCreatorSystem_WEB.Models;

namespace PlcCreatorSystem_WEB.Models.Dto
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
