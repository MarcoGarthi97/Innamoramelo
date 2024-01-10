using Innamoramelo.Controllers;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Innamoramelo.Models
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomUserIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId(HubConnectionContext connection)
        {
            var jsonUserDTO = _httpContextAccessor.HttpContext.Session.GetString("User");
            var userDTO = JsonConvert.DeserializeObject<UserDTO>(jsonUserDTO);

            return userDTO.Id;
        }
    }
}
