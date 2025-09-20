using System.ComponentModel.DataAnnotations;

namespace PlcCreatorSystem_WEB.Models.Dto
{
    public class HmiDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(15)]
        public string IP { get; set; }
        [Required]
        [MaxLength(30)]
        public string Identyfier { get; set; }
        [MaxLength(300)]
        public string Details { get; set; }
        //public Author Author { get; set; }
    }
}
