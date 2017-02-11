using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Cambridge105.SmsApi
{
    internal static class Settings
    {
        public static string SmtpCertificateHash { get { return ConfigurationManager.AppSettings["SmtpCertificateHash"]; } }
        public static string SmtpHost { get { return ConfigurationManager.AppSettings["SmtpHost"]; } }
        public static string SmtpUser { get { return ConfigurationManager.AppSettings["SmtpUser"]; } }
        public static string SmtpPassword { get { return ConfigurationManager.AppSettings["SmtpPassword"]; } }

        public static string MailFrom { get { return ConfigurationManager.AppSettings["MailFrom"]; } }
        public static string MailTo { get { return ConfigurationManager.AppSettings["MailTo"]; } }

        public static string GoogleSheetsApiKey { get { return ConfigurationManager.AppSettings["GoogleSheetsApiKey"]; } }
        public static string GoogleSheetsSpreadsheetId { get { return ConfigurationManager.AppSettings["GoogleSheetsSpreadsheetId"]; } }
    }
}