using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class PlcCreateVM
    {
        public PlcCreateVM() 
        {
            hmiVM = new PlcCreateDTO();
        }
        public PlcCreateDTO hmiVM { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> usersList { get; set; }
    }
}
