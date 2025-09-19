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
        public int PlcID { get; set; }
        [Required]
        public int HmiID { get; set; }
        [Required]
        [MaxLength(300)]
        public string CustomerDetails { get; set; }
        [Required]
        public ProjectStatus Status { get; set; }  // enum
        //public Author Author { get; set; }
    }
}
