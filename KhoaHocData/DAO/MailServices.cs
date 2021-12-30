using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class MailServices
    {
        public async Task<KetQuaTraVe> GuiMailString(string reciepiantMailAddress, string TieuDe, string NoiDung)
        {
            string Body = NoiDung;
            string FromMail = ConfigurationManager.AppSettings["mymail"];
            string MyMailPassword = ConfigurationManager.AppSettings["mymailpassword"];
            string HostMail = "smtp.gmail.com";
            string Subject = TieuDe;
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(FromMail);
                    mail.To.Add(reciepiantMailAddress);
                    mail.Subject = Subject;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    using (SmtpClient client = new SmtpClient(HostMail, 587))
                    {
                        client.Credentials = new System.Net.NetworkCredential(FromMail, MyMailPassword);
                        client.EnableSsl = true;
                        await client.SendMailAsync(mail);
                        return KetQuaTraVe.ThanhCong;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }
    }
}
