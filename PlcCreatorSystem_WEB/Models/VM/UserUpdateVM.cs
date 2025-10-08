using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB.Models.VM
{
    public class UserUpdateVM
    {
        public UserUpdateVM()
        {
            userVM = new UserUpdateDTO();
        }
        public UserUpdateDTO userVM { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> RoleOptions { get; set; }
    }
}
