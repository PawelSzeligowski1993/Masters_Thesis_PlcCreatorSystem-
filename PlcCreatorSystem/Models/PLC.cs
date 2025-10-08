using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlcCreatorSystem_API.Models
{
    public class PLC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Subnet_X1 { get; set; }
        [Required]
        public string IP_X1 { get; set; }
        [Required]
        public string Subnet_X2 { get; set; }
        [Required]
        public string IP_X2 { get; set; }
        [Required]
        public string Identyfier { get; set; }
        public string Details { get; set; }
        [Required]
        [ForeignKey("LocalUser")]
        public int UserID { get; set; }
        public LocalUser LocalUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
