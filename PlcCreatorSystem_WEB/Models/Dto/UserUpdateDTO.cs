using static PlcCreatorSystem_Utility.SD;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PlcCreatorSystem_WEB.Models.Dto
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
