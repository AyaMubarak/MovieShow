using System.Net;
using System.Net.Mail;

namespace MovieShow.Models
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("ayagmk2002@gmail.com", "pudd tjzb ysjn yiml");

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ayagmk2002@gmail.com"),
                    Subject = email.Subject,
                    Body = email.Body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email.Recivers);

                try
                {
                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                }
            }
        }
    }
}
