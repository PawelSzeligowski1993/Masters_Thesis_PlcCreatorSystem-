using System.Net;

namespace PlcCreatorSystem_API.Models
{
    public class APIResponse
    {
        public APIResponse() 
        {
            ErrorsMessages = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorsMessages { get; set; }
        public object Result { get; set; }
    }
}
