using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class HmiDeleteVM
    {
        public HmiDeleteVM() 
        {
            hmiDeleteVM = new HmiDTO();
        }
        public HmiDTO hmiDeleteVM { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> usersList { get; set; }
    }
}
