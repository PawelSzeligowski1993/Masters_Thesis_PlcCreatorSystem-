using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class ProjectDeleteVM
    {
        public ProjectDeleteVM()
        {
            projectVM = new ProjectDTO();
        }
        public ProjectDTO projectVM { get; set; }
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
