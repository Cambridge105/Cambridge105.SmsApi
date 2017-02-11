using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Cambridge105.SmsApi
{
    internal sealed class CertificateValidator
    {
        public void SetupCertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                // Carry on if there's no problems
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;

                // See if it's a trusted certificate
                if (certificate.GetCertHashString() == Settings.SmtpCertificateHash)
                    return true;

                return false;
            };
        }
    }
}