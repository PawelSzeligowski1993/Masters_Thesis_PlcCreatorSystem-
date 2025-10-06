using System.ComponentModel.DataAnnotations;

namespace PlcCreatorSystem_API.Models.Dto
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
