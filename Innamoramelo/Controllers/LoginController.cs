using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;

namespace Innamoramelo.Controllers
{
    public class LoginController : Controller
    {
        private readonly PrivateController _privateController = new();
        public JsonResult Login(string json)
        {
            try
            {
                var user = JsonConvert.DeserializeObject<User>(json);
                if(user != null)
                {
                    Mongo mongo = new Mongo();
                    var findUser = mongo.GetUser(user, true).Result;

                    if (findUser != null && findUser.IsActive == true)
                    {
                        string userJson = JsonConvert.SerializeObject(findUser);
                        HttpContext.Session.SetString("InfoUser", userJson);
                        HttpContext.Session.SetInt32("Logon", 1);

                        var findProfile = mongo.GetProfile(user.Id, 1);
                        if (findProfile != null)
                        {
                            string userProfile = JsonConvert.SerializeObject(findProfile);
                            HttpContext.Session.SetString("ProfileUser", userProfile);
                        }

                        return Json(true);
                    }
                }                
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

                    var insert = mongo.InsertUser(user).Result;
                    if (insert)
                    {
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

        public JsonResult CheckSecretCode(string json)
        {
            try
            {
                var user = _privateController.GetSessionUser();
                var secretCode = JsonConvert.DeserializeObject<SecretCode>(json);

                if (user != null && user.SecretCode != null && secretCode != null)
                {
                    var difference = new DateTime() - user.SecretCode.Created;

                    if (secretCode.Code == user.SecretCode.Code && difference.Value.TotalMinutes < 5)
                    {
                        var updateUser = new User(true);
                        updateUser.Id = user.Id;

                        Mongo mongo = new Mongo();
                        var update = mongo.UpdateUser(updateUser).Result;

                        if (update)
                            return Json(true);
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

            var secretCode = new SecretCode(num.ToString(), new DateTime());

            return secretCode;
        }        
    }
}
