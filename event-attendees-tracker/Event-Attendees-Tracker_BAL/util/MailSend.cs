using System;
using System.Collections.Generic;
using System.Web;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using System.Diagnostics;

namespace Event_Attendees_Tracker_BAL.util
{
  

    public class MailSend
    {
        [DllImport("wininet.dll")]
        public extern static bool InternetGetConnectedState(out int Description, int ReversedValue);

        public static void SendRegistrationMail(List<String> Recepient,String EventName)
        {
            int des;
            if (InternetGetConnectedState(out des, 0))
            {
                Debug.Print(AppDomain.CurrentDomain.BaseDirectory);
                //String FilePath = @"~\..\Event-Attendees-Tracker_BAL\MailTemplate\Register.html";
                //string FilePath = HttpContext.Current.Server.MapPath("~/MailTemplates/Register.html");
                string FilePath = @"C:\Users\Karthik_Patlolla\Desktop\FinalProject\event-attendees-tracker\Event-Attendees-Tracker_BAL\MailTemplate\Register.html";
                //C:\Users\Karthik_Patlolla\Desktop\FinalProject\event-attendees-tracker\Event-Attendees-Tracker_BAL\MailTemplate\Register.html
                StreamReader str = new StreamReader(FilePath);
                string MailBody = str.ReadToEnd();
                MailBody = MailBody.Replace("cid:name", EventName);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("MAIL");
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                mail.From = new MailAddress(Environment.UserName + "@epam.com");
                foreach(string s in Recepient)
                {
                    mail.To.Add(s);
                }
                mail.Subject = EventName+" Registration Mail";
                mail.IsBodyHtml = true;
                SmtpServer.Port = 25;
                SmtpServer.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                string logoPath = HttpContext.Current.Server.MapPath("~/Images/EPAM_LOGO_WhiteAndBlue.png");
                string wallpaper = HttpContext.Current.Server.MapPath("~/Images/EPAM_winter_party-081_72DPIRGB.jpg");
                string ProjectLogo = HttpContext.Current.Server.MapPath("~/Images/ProjectLogo.png");
                mail.AlternateViews.Add(imageEmbedder(logoPath, MailBody, "logo"));
                mail.AlternateViews.Add(imageEmbedder(wallpaper, MailBody, "background"));
                mail.AlternateViews.Add(imageEmbedder(ProjectLogo, MailBody, "ProjectLogo"));
                try
                {
                    SmtpServer.Send(mail);
                }
                catch(Exception ex)
                {

                }
            }
        }

        private static AlternateView imageEmbedder(string SourcePath, string MailBody, string SourceId)
        {
            var htmlView = AlternateView.CreateAlternateViewFromString(MailBody, null, "text/html");
            var resource = new LinkedResource(SourcePath) { ContentId = SourceId };
            resource.TransferEncoding = TransferEncoding.Base64;
            htmlView.LinkedResources.Add(resource);
            return htmlView;
        }
    }
}
