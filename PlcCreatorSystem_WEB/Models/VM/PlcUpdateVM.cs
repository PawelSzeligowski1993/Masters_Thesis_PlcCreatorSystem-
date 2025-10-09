using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class PlcUpdateVM
    {
        public PlcUpdateVM() 
        {
            plcUpdateVM = new PlcUpdateDTO();
        }
        public PlcUpdateDTO plcUpdateVM { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> usersList { get; set; }
    }
}
