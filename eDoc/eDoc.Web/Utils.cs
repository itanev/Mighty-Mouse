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

    }
}







