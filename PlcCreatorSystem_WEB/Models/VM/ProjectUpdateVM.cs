using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class ProjectUpdateVM
    {
        public ProjectUpdateVM()
        {
            project = new ProjectUpdateDTO();
        }
        public ProjectUpdateDTO project { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> plcList { get; set; } = Enumerable.Empty<SelectListItem>();
        [ValidateNever]
        public IEnumerable<SelectListItem> hmiList { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
