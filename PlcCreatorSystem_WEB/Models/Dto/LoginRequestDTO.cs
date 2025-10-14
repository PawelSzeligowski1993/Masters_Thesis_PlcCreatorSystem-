using System.ComponentModel.DataAnnotations;

namespace PlcCreatorSystem_WEB.Models.Dto
{
    public class LoginRequestDTO
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
