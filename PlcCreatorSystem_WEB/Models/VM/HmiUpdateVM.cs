using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class HmiUpdateVM
    {
        public HmiUpdateVM() 
        {
            hmiUpdateVM = new HmiUpdateDTO();
        }
        public HmiUpdateDTO hmiUpdateVM { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> usersList { get; set; }
    }
}
