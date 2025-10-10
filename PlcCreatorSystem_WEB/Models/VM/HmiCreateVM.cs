using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class HmiCreateVM
    {
        public HmiCreateVM() 
        {
            hmiCreateVM = new HmiCreateDTO();
        }
        public HmiCreateDTO hmiCreateVM { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> usersList { get; set; }
    }
}
