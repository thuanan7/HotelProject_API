﻿using HotelProject_Utility;
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
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClient, ITokenProvider tokenProvider)
        {
            this.responseModel = new APIResponse();
            this.httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }


        public async Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true)
        {
            try
            {
                var client = httpClient.CreateClient("HotelProject");

                HttpRequestMessage message = new();

                if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*");
                }
                else
                {
                    message.Headers.Add("Accept", "application/json");
                }

                message.RequestUri = new Uri(apiRequest.Url);
                if (withBearer && _tokenProvider.GetToken() != null)
                {
                    var token = _tokenProvider.GetToken();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                }

                if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();
                    foreach(var prop in apiRequest.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(apiRequest.Data);
                        if (value is FormFile)
                        {
                            var file = (FormFile)value;
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                        }
                    }
                    message.Content = content;
                }
                else
                {
                    if (apiRequest.Data != null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                    }
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
