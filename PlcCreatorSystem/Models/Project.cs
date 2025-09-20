using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static PlcCreatorSystem_Utility.SD;

namespace PlcCreatorSystem_API.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [ForeignKey("PLC")]
        public int PlcID { get; set; }
        public PLC PLC { get; set; }
        [ForeignKey("HMI")]
        public int HmiID { get; set; }
        public HMI HMI { get; set; }
        [Required]
        public string CustomerDetails { get; set; }
        [Required]
        public ProjectStatus Status { get; set; }  // enum
        //[ForeignKey("Author")]
        //public int AuthorID { get; set; }
        //public Author Author { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
