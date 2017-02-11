using Cambridge105.SmsApi.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Cambridge105.SmsApi.Controllers
{
    public class SmsController : Controller
    {
        private static NameMap s_NameMap = new NameMap(Settings.GoogleSheetsApiKey, Settings.GoogleSheetsSpreadsheetId);

        [HttpPost]
        public ActionResult Index()
        {
            string timestamp = Request.Params["scts"];
            string sender = Request.Params["oa"];
            string content = Request.Params["ud"];

            if (sender != null)
            {
                sender = sender.Trim('+', ' ');
                sender = s_NameMap.LookupNumber(sender);
            }

            MailMessage mm = new MailMessage(Settings.MailFrom, Settings.MailTo);
            mm.Subject = "SMS from " + sender;
            mm.Body = content ?? "Empty message";
            SmtpClient client = new SmtpClient(Settings.SmtpHost, 587);
            client.Credentials = new NetworkCredential("smtprelay", "sendnow");
            client.EnableSsl = true;
            client.DeliveryFormat = SmtpDeliveryFormat.International;
            client.Send(mm);

            return Content("OK");

        }
    }
}