using System;
using System.Text;

namespace nanoFramework.Telegram.Bot.Extensions
{
    internal static class StringExtensions
    {
        public static string DecodeUnicode(this string input)
        {
            var decoded = new StringBuilder();
            var length = input.Length;
            var i = 0;

            while (i < length)
            {
                if (i + 5 < length && input[i] == '\\' && input[i + 1] == 'u')
                {
                    var hexCode = input.Substring(i + 2, 4);
                    try
                    {
                        int unicodeValue = Convert.ToInt32(hexCode, 16);
                        decoded.Append((char)unicodeValue);
                        i += 6;
                    }
                    catch
                    {
                        decoded.Append(input.Substring(i, 6));
                        i += 6;
                    }
                }
                else
                {
                    decoded.Append(input[i]);
                    i++;
                }
            }

            return decoded.ToString();
        }
    }
}
