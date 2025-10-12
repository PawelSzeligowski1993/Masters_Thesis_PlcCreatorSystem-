using System.ComponentModel.DataAnnotations;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class UploadCsvVM
    {
        [Required]
        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "CSV file")]
        public IFormFile CsvFile { get; set; }
    }
}
