using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDoc.Web
{
    public class Utils
    {
        public static string GetConfirmationCode(object seed, int length, bool useEnvTickCount = true)
        {
            int seedInt = seed.GetHashCode();
            if (useEnvTickCount) seedInt += Environment.TickCount;
            var random = new Random(seedInt);
            var array = new byte[length];
            random.NextBytes(array);
            return Convert.ToBase64String(array).Substring(0, length);
        }

        public static byte[] GetTokenAssembly(string secret)
        {
            var tree = SyntaxTree.ParseText(
                string.Format(GetTokenConfirmationCodeSource, secret));
            IEnumerable<Diagnostic> _;
            return TeamAzureDragon.CSharpCompiler.Compiler
                .CompileToAssembly(tree, out _);
        }

        public static string GetTokenConfirmationCode(string code)
        {
            return GetConfirmationCode(code, code.Length, false);
        }

        public const string GetTokenConfirmationCodeSource =
@"using System;

public class Program
{
const string SECRET = ""{0}"";
public static void Main(string[] args)
{
    Console.WriteLine(""Enter token code:"");
    string code = Console.ReadLine().Trim();
    Console.WriteLine(GetConfirmationCode(code));
}
public static string GetConfirmationCode(string code)
{
    var random = new Random((code + SECRET).GetHashCode());
    var array = new byte[code.Length];
    random.NextBytes(array);
    return Convert.ToBase64String(array).Substring(0, length);
}
}
";
        public static void SendEmail(string to, string subject, string body)
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


            message.Subject =
            message.Body = body;

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







