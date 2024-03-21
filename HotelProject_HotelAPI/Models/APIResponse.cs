using System.Net;

namespace HotelProject_HotelAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
        public APIResponse()
        {
            IsSuccess = true;
        }
    }
}
