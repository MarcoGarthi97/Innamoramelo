using MimeKit;
using MailKit.Net.Smtp;

namespace Innamoramelo.Models
{
    public class Google
    {
        private readonly string[] _mail = File.ReadAllText(@"C:\Users\marco\source\repos\_MyCredentials\Gmail.txt").Split(';'); //user;AppPassword

        internal bool SendMail(SendMail sendMail)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("Noreplay", _mail[0]));
                email.To.Add(new MailboxAddress(sendMail.Mail, sendMail.Mail));

                email.Subject = sendMail.Object;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = sendMail.Body
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

    public class SendMail
    {
        public string? Mail { get; set; }
        public string? Object { get; set; }
        public string? Body { get; set; }

        public SendMail() { }
        public SendMail(string? mail, string? _object, string? body)
        {
            Mail = mail;
            Object = _object;
            Body = body;
        }
    }
}
