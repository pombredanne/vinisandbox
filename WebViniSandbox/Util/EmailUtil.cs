using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Ionic.Zip;
using ViniSandbox.Models;
using System.Configuration;
using System.Net.Configuration;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net;

namespace WebViniSandbox.Util
{
    public class EmailUtil
    {
        private static Stream GetZipStream(file file)
        {
            MemoryStream ms = new MemoryStream();
            string path = Environment.GetEnvironmentVariable("temp") + "\\" + file.name;
            using (FileStream sw = new FileStream(path, FileMode.Create))
            {
                byte[] data = file.file_detail.data;
                sw.Write(data, 0, data.Length);
            }
            using (ZipFile zip = new ZipFile())
            {
                zip.Password = "infected";
                zip.AddItem(path, "");
                zip.Save(ms);
            }
            System.IO.File.Delete(path);
            return ms;
        }

        public static void EnviaMensagemComAnexos(file file, string[] emails)
        {
            Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("~");
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

            if (mailSettings != null)
            {
                string host = mailSettings.Smtp.Network.Host;
                string password = mailSettings.Smtp.Network.Password;
                string username = mailSettings.Smtp.Network.UserName;
                int port = mailSettings.Smtp.Network.Port;
                bool enableSSL = mailSettings.Smtp.Network.EnableSsl;

                Attachment anexo = new Attachment(GetZipStream(file), "submit.zip");

                SmtpClient client = new SmtpClient(host, port);
                client.EnableSsl = enableSSL;
                client.Credentials = new NetworkCredential(username, password);
                //client.Credentials = cred;
                //client.UseDefaultCredentials = true;

                MailMessage message = new MailMessage();
                message.From = new MailAddress("vinisandbox@aol.com", "V1N1S4NDB0X");
                message.Attachments.Add(anexo);
                emails = new string[] { "mviniciusleal@gmail.com" };
                foreach (var email in emails)
                {
                    message.To.Add(email);
                }
                client.Send(message);                
            }
        }
    }
}