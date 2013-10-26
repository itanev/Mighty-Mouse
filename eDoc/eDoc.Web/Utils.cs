using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDoc.Web
{
    public class Utils
    {
        public static string GetConfirmationCode(object seed, int length)
        {
            var random = new Random(seed.GetHashCode() + Environment.TickCount);
            var array = new char[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = (char)random.Next((int)'a', (int)'z');
            }
            return new string(array);
        }
    }
}