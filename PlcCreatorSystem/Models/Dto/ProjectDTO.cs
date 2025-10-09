using static PlcCreatorSystem_Utility.SD;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlcCreatorSystem_API.Models.Dto
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public int PlcID { get; set; }
        public PlcDTO PLC { get; set; }
        [Required]
        public int HmiID { get; set; }
        public HmiDTO HMI { get; set; }
        [Required]
        public string CustomerDetails { get; set; }
        [Required]
        public ProjectStatus Status { get; set; }  // enum
        [Required]
        public int UserID { get; set; }
        public UserDTO LocalUser { get; set; }
    }
}
