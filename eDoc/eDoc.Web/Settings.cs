using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace eDoc.Web
{
    public static class Settings
    {
        static readonly XmlDocument settings = new XmlDocument();

        public static void Initialize()
        {   
            settings.Load(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\settings.xml");
        }


        public static void GetSmsSettings(out string fromNumber, out string accountSid, out string authToken)
        {
            fromNumber = settings.SelectSingleNode("/settings/fromNumber").InnerXml.Trim();
            accountSid = settings.SelectSingleNode("/settings/accountSid").InnerXml.Trim();
            authToken = settings.SelectSingleNode("/settings/authToken").InnerXml.Trim();
        }

        public static void GetEmailSettings(out string mailgunAccount, out string mailgunKey, out string fromEmail)
        {
            mailgunAccount = settings.SelectSingleNode("/settings/mailgunAccount").InnerXml.Trim();
            mailgunKey = settings.SelectSingleNode("/settings/mailgunKey").InnerXml.Trim();
            fromEmail = settings.SelectSingleNode("/settings/fromEmail").InnerXml.Trim();
        }
    }
}