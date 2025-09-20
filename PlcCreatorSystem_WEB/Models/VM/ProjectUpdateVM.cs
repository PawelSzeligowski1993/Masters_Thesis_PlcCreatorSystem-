using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class ProjectUpdateVM
    {
        public ProjectUpdateVM()
        {
            Project = new ProjectUpdateDTO();
        }
        public ProjectUpdateDTO Project { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> PLCList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> HMIList { get; set; }
    }
}
