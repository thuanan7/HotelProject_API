using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace HotelProject_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new APIResponse();
            this.httpClient = httpClient;
        }


        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("HotelProject");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                HttpResponseMessage apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse APIResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (APIResponse == null) 
                    {
                        APIResponse = new APIResponse();
                        APIResponse.ErrorMessage = "Error: Something Wrong!";
                        APIResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        APIResponse.IsSuccess = false;
                    }
                    else if (APIResponse.StatusCode == System.Net.HttpStatusCode.BadRequest
                        || APIResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        APIResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        APIResponse.IsSuccess = false;
                    }
                    var res = JsonConvert.SerializeObject(APIResponse);
                    var returnObj = JsonConvert.DeserializeObject<T>(res);
                    return returnObj;
                }
                catch (Exception e)
                {
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessage = ex.Message,
                    IsSuccess = false,
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}
