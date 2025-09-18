using PlcCreatorSystem_API.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlcCreatorSystem_API.Models.Dto
{
    public class ProjectCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public PLC PLC { get; set; }
        [Required]
        public HMI HMI { get; set; }
        [Required]
        [MaxLength(300)]
        public string CustomerDetails { get; set; }
        [Required]
        public ProjectStatus Status { get; set; }  // enum
        //public Author Author { get; set; }
    }
}
