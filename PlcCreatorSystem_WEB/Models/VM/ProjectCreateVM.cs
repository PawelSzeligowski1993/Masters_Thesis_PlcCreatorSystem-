using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class ProjectCreateVM
    {
        public ProjectCreateVM()
        {
            project = new ProjectCreateDTO();
        }
        public ProjectCreateDTO project { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> plcList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> hmiList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> statusList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> usersList { get; set; }
    }
}
