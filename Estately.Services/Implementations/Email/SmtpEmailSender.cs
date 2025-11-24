//using Microsoft.Extensions.Options;
//using Estately.Services.Interfaces.Email;
//using System.Net;
//using System.Net.Mail;

//namespace Estately.Services.Implementations
//{
//    public class SmtpOptions
//    {
//        public string Host { get; set; } = "";
//        public int Port { get; set; } = 587;
//        public bool EnableSsl { get; set; } = true;
//        public string UserName { get; set; } = "";
//        public string Password { get; set; } = "";
//        public string From { get; set; } = "";
//    }

//    public class SmtpEmailSender : IEmailSender
//    {
//        private readonly SmtpOptions _options;
//        public SmtpEmailSender(IOptions<SmtpOptions> options)
//        {
//            _options = options.Value;
//        }

//        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
//        {
//            using var client = new SmtpClient(_options.Host, _options.Port)
//            {
//                EnableSsl = _options.EnableSsl,
//                Credentials = new NetworkCredential(_options.UserName, _options.Password)
//            };

//            var mail = new MailMessage(_options.From, email, subject, htmlMessage)
//            {
//                IsBodyHtml = true
//            };

//            await client.SendMailAsync(mail);
//        }
//    }
//}
