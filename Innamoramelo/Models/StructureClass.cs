using Innamoramelo.Controllers;

namespace Innamoramelo.Models
{
    public class Authentication
    {
        public int NotLogged { get; set; }
        public int CreateProfile { get; set; }
        public int Logged { get; set; }

        public Authentication()
        {
            NotLogged = 0;
            CreateProfile = -1;
            Logged = 1;
        }

        internal int GetAuthentication(HttpContext _httpContext)
        {
            PrivateController _privateController = new(_httpContext);

            int? logon = _privateController.GetLogon();
            if (logon == null || logon == 0)
                return NotLogged;

            bool? createProfile = _privateController.GetSessionUser().CreateProfile;
            if (createProfile!= null && createProfile.Value)
                return Logged;
            else
                return CreateProfile;
        }

        internal string GetSite(string site, HttpContext _httpContext)
        {
            int val = GetAuthentication(_httpContext);

            switch (val)
            {
                case 1:
                    return site;
                case -1:
                    return "Profile";
                default:
                    return "Login";
            }
        }
    }
}
