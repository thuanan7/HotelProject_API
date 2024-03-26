using HotelProject_Utility;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services.IServices;

namespace HotelProject_Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.AccessToken);
        }

        public TokenDTO GetToken()
        {
            try
            {
                bool hasAccessToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string accessToken);
                TokenDTO token = new TokenDTO()
                {
                    AccessToken = accessToken,
                };
                return hasAccessToken ? token : null;
            }
            catch 
            { 
                return null;
            }
        }

        public void SetToken(TokenDTO tokenDTO)
        {
            var cookiesOptions = new CookieOptions { Expires = DateTime.UtcNow.AddMinutes(60) };
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.AccessToken, tokenDTO.AccessToken, cookiesOptions);
        }
    }
}
