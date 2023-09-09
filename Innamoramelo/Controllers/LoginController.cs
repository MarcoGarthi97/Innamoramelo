using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Innamoramelo.Controllers
{
    public class LoginController : Controller
    {
        private PrivateController _privateController;
        private HttpContext LoadContext()
        {
            return HttpContext;
        }

        public JsonResult Login(string json)
        {
            try
            {
                var user = JsonConvert.DeserializeObject<User>(json);
                bool result = GetLogin(user);
                
                return Json(result);
            }
            catch (Exception ex)
            {
                
            }

            return Json(false);
        }

        public JsonResult Register(string json)
        {
            try
            {
                var user = JsonConvert.DeserializeObject<User>(json);

                Mongo mongo = new Mongo();
                var findUser = mongo.GetUser(user, true).Result;

                if (findUser == null)
                {
                    user.SecretCode = CreateSecretCode();
                    user.IsActive = false;
                    user.CreateProfile = false;

                    _privateController = new(LoadContext());

                    var insert = mongo.InsertUser(user).Result;
                    if (insert)
                    {
                        json = JsonConvert.SerializeObject(user);
                        _privateController.PutSessionUser(json);

                        SendCode();
                        return Json(true);
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return Json(false);
        }

        public JsonResult CheckSecretCode(string json)
        {
            try
            {
                _privateController = new(LoadContext());

                var user = _privateController.GetSessionUser();
                var secretCode = JsonConvert.DeserializeObject<SecretCode>(json);

                if (user != null && user.SecretCode != null && secretCode != null)
                {
                    DateTime now = DateTime.Now;
                    var difference = now - user.SecretCode.Created;

                    if (secretCode.Code == user.SecretCode.Code && difference.Value.TotalMinutes < 5)
                    {
                        user.IsActive = true;

                        Mongo mongo = new Mongo();
                        var update = mongo.UpdateUser(user).Result;

                        if (update)
                        {
                            string jsonUser = JsonConvert.SerializeObject(user);
                            _privateController.PutSessionUser(jsonUser);

                            return Json(true);
                        }
                            
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(false);
        }

        public JsonResult ResendCode()
        {
            try
            {                
                var _user = _privateController.GetSessionUser();

                var user = new User();
                user.Id = _user.Id;
                user.SecretCode = CreateSecretCode();

                Mongo mongo = new Mongo();
                var insert = mongo.UpdateUser(user).Result;
                if (insert)
                {
                    _user = mongo.GetUser(user, true).Result;
                    if(_user != null)
                    {
                        string json = JsonConvert.SerializeObject(_user);
                        HttpContext.Session.SetString("InfoUser", json);

                        SendCode();

                        return Json(true);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(false);
        }

        public JsonResult Logout()
        {
            HttpContext.Session.Clear();

            return Json(true);
        }

        private bool GetLogin(User user)
        {
            if (user != null)
            {
                Mongo mongo = new Mongo();
                var findUser = mongo.GetUser(user, true).Result;

                if (findUser != null && findUser.IsActive == true)
                {
                    string userJson = JsonConvert.SerializeObject(findUser);

                    _privateController = new(LoadContext());
                    _privateController.PutSessionUser(userJson);
                    _privateController.PutLogon(1);

                    return true;
                }
            }

            return false;
        }

        private void SendCode()
        {
            var user = _privateController.GetSessionUser();

            string body = "Thank you for registering for Innamoramelo.\r\n\r\nTo complete your registration, please enter the following code in the registration form:\r\n\r\n" + user.SecretCode.Code + "\r\n\r\nIf you have any questions, please do not hesitate to contact us.\r\n\r\nThank you!";
            var sendMail = new SendMail(user.Email, "Innamoramelo: Code registration", body);

            var google = new Google();
            google.SendMail(sendMail);
        }

        private SecretCode CreateSecretCode()
        {
            Random rnd = new Random();
            int num = rnd.Next(10000, 99999);

            var secretCode = new SecretCode(num.ToString(), DateTime.Now);

            return secretCode;
        }        
    }
}
