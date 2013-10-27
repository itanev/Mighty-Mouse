using eDoc.Models;
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
        static string path;

        public static void Initialize()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\settings.xml";
            settings.Load(path);
        }

        public static void Save()
        {
            settings.Save(path);
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

        public static bool Validate(Document doc)
        {
            var result = true;

            if (ValidateToken)
            {
                if (!doc.TokenValidated) return false;
            }

            if (ValidateEmail)
            {
                if (!doc.EmailValidated) return false;
            }

            if (ValidateSms)
            {
                if (!doc.PhoneValidated) return false;
            }

            return result;
        }

        public static bool ValidateToken
        {
            get
            {
                return settings.SelectSingleNode("/settings/validateToken").InnerXml.Trim().ToLower() == "true";
            }
            set
            {
                settings.SelectSingleNode("/settings/validateToken").InnerXml = value + "";
            }
        }

        public static bool ValidateSms
        {
            get
            {
                return settings.SelectSingleNode("/settings/validateSms").InnerXml.Trim().ToLower() == "true";
            }
            set
            {
                settings.SelectSingleNode("/settings/validateSms").InnerXml = value + "";
            }
        }

        public static bool ValidateEmail
        {
            get
            {
                return settings.SelectSingleNode("/settings/validateEmail").InnerXml.Trim().ToLower() == "true";
            }
            set
            {
                settings.SelectSingleNode("/settings/validateEmail").InnerXml = value + "";
            }
        }
    }
}