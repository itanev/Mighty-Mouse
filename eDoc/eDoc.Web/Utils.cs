using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Typesafe.Mailgun;

namespace eDoc.Web
{
    public class Utils
    {
        public static int GetHash(string str)
        {
            int result = 0;
            foreach (var ch in str)
            {
                unchecked
                {
                    result += ch;
                    result <<= 2;
                }
            }
            return result;
        }
        public static string GetConfirmationCode(string seed, int length, bool useEnvTickCount = true)
        {
            int seedInt = GetHash(seed);
            if (useEnvTickCount) seedInt += Environment.TickCount;
            var random = new Random(seedInt);
            var array = new byte[length];
            random.NextBytes(array);
            return Convert.ToBase64String(array).Substring(0, length);
        }

        public static byte[] GetTokenAssembly(string secret)
        {
            var source = GetTokenConfirmationCodeSource.Replace("####", secret);
            var tree = SyntaxTree.ParseText(source);
            IEnumerable<Diagnostic> _;
            return TeamAzureDragon.CSharpCompiler.Compiler
                .CompileToAssembly(tree, out _);
        }

        public static string GetTokenConfirmationCode(string userName, string tokenInput)
        {
            return GetConfirmationCode(tokenInput + userName, tokenInput.Length, false);
        }

        public const string GetTokenConfirmationCodeSource =
@"using System;

public class Program
{
const string SECRET = ""####"";
public static void Main(string[] args)
{
    Console.WriteLine(""Enter token code:"");
    string tokenInput = Console.ReadLine().Trim();
    Console.WriteLine(GetConfirmationCode(tokenInput));
}
public static int GetHash(string str)
{
    int result = 0;
    foreach (var ch in str)
    {
        unchecked
        {
            result += ch;
            result <<= 2;
        }
    }
    return result;
}
public static string GetConfirmationCode(string tokenInput)
{
    Console.WriteLine(tokenInput + SECRET);
    Console.WriteLine(GetHash(tokenInput + SECRET));
    var random = new Random(GetHash(tokenInput + SECRET));
    var array = new byte[tokenInput.Length];
    random.NextBytes(array);
    return Convert.ToBase64String(array).Substring(0, tokenInput.Length);
}
}
";
        public static void SendEmail(string to, string subject, string body, string addressee = null)
        {
            // https://api.mailgun.net/v2
            // http://documentation.mailgun.com/quickstart.html#sending-messages
            string mailgunAccount;
            string mailgunKey;
            string fromEmail;
            Settings.GetEmailSettings(out mailgunAccount, out mailgunKey, out fromEmail);
            var client = new MailgunClient(mailgunAccount, mailgunKey);

            var message = new System.Net.Mail.MailMessage(fromEmail, to);
            message.Sender = new MailAddress(fromEmail);

            message.From = message.Sender;
            message.IsBodyHtml = true;
            message.Subject = "MightyMouse Documents System - " + subject;
            if (addressee != null)
            {
                body = "<p>Dear " + addressee + ",</p>" + body;
            }
            message.Body = body + 
@"<p>Love,
<br>
The MightyMouse Team</p>
<br>
<p>Please don't reply to this email as the inbox is not monitored.</p>";

            client.SendMail(message);
        }

        public static void SendSms(string toNumber, string body)
        {
            string fromNumber;
            string accountSid;
            string authToken;
            Settings.GetSmsSettings(out fromNumber, out accountSid, out authToken);
            var smsClient = new Twilio.TwilioRestClient(accountSid, authToken);
            smsClient.SendSmsMessage(fromNumber, toNumber, body);

        }

    }
}







