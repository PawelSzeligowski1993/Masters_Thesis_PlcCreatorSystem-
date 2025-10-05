using PlcCreatorSystem_Utility;
using static PlcCreatorSystem_Utility.SD;

namespace PlcCreatorSystem_WEB.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string? Url { get; set; }
        public object? Data { get; set; }
        public string Token { get; set; }
    }
}
