using static HotelProject_Utility.SD;

namespace HotelProject_Web.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; }
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
        public APIRequest()
        {
            ApiType = ApiType.GET;
        }
    }
}
