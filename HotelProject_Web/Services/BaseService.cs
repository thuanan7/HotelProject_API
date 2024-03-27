using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using HotelProject_Web.Models.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotelProject_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        private readonly ITokenProvider _tokenProvider;
        protected readonly string HotelApiUrl;
        private IHttpContextAccessor _httpContextAccessor;
        public BaseService(IHttpClientFactory httpClient, ITokenProvider tokenProvider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.responseModel = new APIResponse();
            this.httpClient = httpClient;
            _tokenProvider = tokenProvider;
            HotelApiUrl = configuration.GetValue<string>("ServiceUrls:HotelProjectAPI");
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true)
        {
            try
            {
                var client = httpClient.CreateClient("HotelProject");

                var messageFactory = () =>
                {
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

                    if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
                    {
                        var content = new MultipartFormDataContent();
                        foreach (var prop in apiRequest.Data.GetType().GetProperties())
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
                    return message;
                };

                HttpResponseMessage apiResponse = await SendWithRefreshTokenAsync(client, messageFactory, withBearer);
                                
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
                catch
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

        private async Task<HttpResponseMessage> SendWithRefreshTokenAsync(HttpClient httpClient,
            Func<HttpRequestMessage> httpRequestMessageFactory, bool withBearer = true)
        {
            if (!withBearer)
            {
                return await httpClient.SendAsync(httpRequestMessageFactory());
            }

            TokenDTO tokenDTO = _tokenProvider.GetToken();
            if (tokenDTO != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDTO.AccessToken);
            }

            try
            {
                var response = await httpClient.SendAsync(httpRequestMessageFactory());
                if (response.IsSuccessStatusCode)
                    return response;

                // If this fails then pass refresh token!
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await InvokeRefreshTokenEndpoint(httpClient, tokenDTO);
                    response = await httpClient.SendAsync(httpRequestMessageFactory());
                    return response;
                }
                return response;
            }
            catch (HttpRequestException httpRequestException)
            {
                if (httpRequestException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await InvokeRefreshTokenEndpoint(httpClient, tokenDTO);
                    return await httpClient.SendAsync(httpRequestMessageFactory());
                }
                throw;
            }
        }

        private async Task InvokeRefreshTokenEndpoint(HttpClient httpClient, TokenDTO tokenDTO)
        {
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri($"{HotelApiUrl}/api/User/refresh");
            message.Method = HttpMethod.Post;
            message.Content = new StringContent(JsonConvert.SerializeObject(tokenDTO), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(message);

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(content);

            if (apiResponse?.IsSuccess == false)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync();
                _tokenProvider.ClearToken();
            }
            else
            {
                var tokenDataStr = JsonConvert.SerializeObject(apiResponse.Result);
                var tokenResponse = JsonConvert.DeserializeObject<TokenDTO>(tokenDataStr);
                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    // New method to sign in with the new token 
                    await SignInWithNewTokens(tokenResponse);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                }
            }
        }

        private async Task SignInWithNewTokens(TokenDTO tokenDTO)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenDTO.AccessToken);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            _tokenProvider.SetToken(tokenDTO);
        }
    }
}
