using System;
using System.Globalization;
using System.Text;

namespace ValisReporter
{
    /// <summary>
    /// Utilities for the ValisManager
    /// </summary>
    public static class Utilities
    {
        public const string DateTime_Format_General = "dd/MM/yyyy HH:mm:ss";
        internal const string DateTime_Format_General_NO_Seconds = "dd/MM/yyyy HH:mm";
        internal const string DateTime_SqlFormat = "yyyy/MM/dd HH:mm:ss";
        internal const String DateTime_Format_NoTime = "dd/MM/yyyy";
        internal const string DateTime_Format_MonthYear = "MM/yyyy";
        internal const string DateTime_Format_Human = "d MMM yyyy, HH:mm";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="format"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string input, string format, out DateTime result)
        {
            result = default(DateTime);
            try
            {
                result = DateTime.ParseExact(input, format, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        /// is essentially a JSON string.
        /// 
        /// The string returned includes outer quotes 
        /// Example Output: "Hello \"Rick\"!\r\nRock on"
        /// <para>http://weblog.west-wind.com/posts/2007/Jul/14/Embedding-JavaScript-Strings-from-an-ASPNET-Page</para>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\'':
                        sb.Append("\\\'");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }

    }
}