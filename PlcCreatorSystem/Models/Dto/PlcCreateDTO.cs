using System.ComponentModel.DataAnnotations;

namespace PlcCreatorSystem_API.Models.Dto
{
    public class PlcCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Subnet_X1 { get; set; }
        [Required]
        [MaxLength(15)]
        public string IP_X1 { get; set; }
        [Required]
        [MaxLength(30)]
        public string Subnet_X2 { get; set; }
        [Required]
        [MaxLength(15)]
        public string IP_X2 { get; set; }
        [Required]
        [MaxLength(30)]
        public string Identyfier { get; set; }
        public string Details { get; set; }
        [Required]
        public int UserID { get; set; }
    }
}
