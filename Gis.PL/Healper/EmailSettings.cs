using System.Net;
using System.Net.Mail;

namespace Gis.PL.Healper
{
    public static class EmailSettings
    {
        
        public static bool SendEmail(Email email)
        {
            //  mail Server : Gmail
            //  SMTP   

            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("omaralshandidy@gmail.com", "bkmydvkupbulwbjt  ");  //  Sender
                client.Send("omaralshandidy@gmail.com", email.To, email.Subject, email.Body);
                return true;

            }
            catch (Exception )
            {
                return false;
            }
        }
    }
}
