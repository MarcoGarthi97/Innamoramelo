using MimeKit;
using MailKit.Net.Smtp;

namespace InnamorameloAPI.Models
{
    public class GoogleAPI
    {
        private readonly string[] _mail = File.ReadAllText(@"C:\Users\marco\source\repos\_MyCredentials\Innamoramelo\Gmail.txt").Split(';'); //user;AppPassword

        internal bool SendMail(string emailUser, string code)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("Noreplay", _mail[0]));
                email.To.Add(new MailboxAddress(emailUser, emailUser));

                email.Subject = "Innamoramelo: Code registration";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "Thank you for registering for Innamoramelo.\r\n\r\nTo complete your registration, please enter the following code in the registration form:\r\n\r\n" 
                    + code + "\r\n\r\nIf you have any questions, please do not hesitate to contact us.\r\n\r\nThank you!"
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    smtp.Authenticate(_mail[0], _mail[1]);

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }
    }
}
