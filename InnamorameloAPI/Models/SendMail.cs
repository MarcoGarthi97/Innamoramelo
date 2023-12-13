namespace InnamorameloAPI.Models
{
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
