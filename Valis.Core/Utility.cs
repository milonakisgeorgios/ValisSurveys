using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Utility
    {

        #region system-wide default Security Policy values
        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// <para>Default Value: false</para>
        /// </summary>
        internal static Boolean EnablePasswordRetrieval = false;
        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// <para>Default Value: false</para>
        /// </summary>
        internal static Boolean EnablePasswordReset = false;
        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// <para>Default Value: false</para>
        /// </summary>
        internal static Boolean RequiresQuestionAndAnswer = false;
        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// <para>Default Value: true</para>
        /// </summary>
        internal static Boolean RequiresUniqueEmail = true;
        /// <summary>
        /// Gets a value indicating the format for storing passwords in the membership data store.
        /// <para>Default Value: LrPasswordFormat.Hashed</para>
        /// </summary>
        internal static VLPasswordFormat PasswordFormat = VLPasswordFormat.Hashed;
        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// <para>Default Value: 5</para>
        /// </summary>
        internal static System.Int16 MaxInvalidPasswordAttempts = 5;
        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// <para>Default Value: 10</para>
        /// </summary>
        internal static System.Int16 PasswordAttemptWindow = 10;
        /// <summary>
        /// Gets the minimum length required for a password.
        /// <para>Default Value: 4</para>
        /// </summary>
        internal static System.Int16 MinRequiredPasswordLength = 4;
        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// <para>Default Value: 0</para>
        /// </summary>
        internal static System.Int16 MinRequiredNonAlphanumericCharacters = 0;
        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// <para>Default Value: string.Empty</para>
        /// </summary>
        internal static string PasswordStrengthRegularExpression = string.Empty;
        #endregion


        #region EmailCheck utilites
        static Regex ValidEmailRegex = CreateValidEmailRegex();
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        internal static bool EmailIsValid(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static bool IsValidDomainName(string name)
        {
            return Uri.CheckHostName(name) != UriHostNameType.Unknown;
        }
        #endregion




        /// <summary>
        /// Ελέγχει την εγκυρότητα μίας αλφαριθμητικής παραστάσεως.
        /// <para>Εάν ο έλεγχος είναι επιτυχής επιστρέφει true ειδάλλως επιστρέφει false.</para>
        /// </summary>
        /// <param name="param"></param>
        /// <param name="checkForNull">Ελέγχει η τιμή να μην είναι null.</param>
        /// <param name="checkIfEmpty">Ελέγχει η τιμή να μην είναι κενή.</param>
        /// <param name="checkForCommas">Ελέγχει η τιμή να μην περιέχει κόματα.</param>
        /// <param name="maxSize">Το μέγιστο επιτρέπομενο μήκος (σε χαρακτήρες) της τιμής.</param>
        /// <returns></returns>
        internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize)
        {
            if (param == null)
            {
                return !checkForNull;
            }

            param = param.Trim();
            if ((checkIfEmpty && param.Length < 1) ||
                 (maxSize > 0 && param.Length > maxSize) ||
                 (checkForCommas && param.Contains(",")))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Ελέγχει την εγκυρότητα μίας αλφαριθμητικής παραστάσεως.
        /// <para>Εάν ο έλεγχος δεν είναι επιτυχής ρίχνει ένα exception!</para>
        /// </summary>
        /// <param name="param"></param>
        /// <param name="checkForNull">Ελέγχει η τιμή να μην είναι null.</param>
        /// <param name="checkIfEmpty">Ελέγχει η τιμή να μην είναι κενή.</param>
        /// <param name="checkForCommas">Ελέγχει η τιμή να μην περιέχει κόματα.</param>
        /// <param name="maxSize">Το μέγιστο επιτρέπομενο μήκος (σε χαρακτήρες) της τιμής.</param>
        /// <param name="paramName"></param>
        internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
            {
                if (checkForNull)
                {
                    throw new ArgumentNullException(paramName);
                }

                return;
            }

            param = param.Trim();
            if (checkIfEmpty && param.Length < 1)
            {
                throw new ArgumentException(SR.GetString(SR.Parameter_can_not_be_empty, paramName), paramName);
            }

            if (maxSize > 0 && param.Length > maxSize)
            {
                throw new ArgumentException(SR.GetString(SR.Parameter_too_long, paramName, maxSize.ToString(CultureInfo.InvariantCulture)), paramName);
            }

            if (checkForCommas && param.Contains(","))
            {
                throw new ArgumentException(SR.GetString(SR.Parameter_can_not_contain_comma, paramName), paramName);
            }
        }

        /// <summary>
        /// Ελέγχει τις παραμέτρους για την διαδικασία του paging. 
        /// <para>To pageIndex πρέπει να αρχίζει απο την μονάδα (1).</para>
        /// <para>To pageSize πρέπει να είναι ένα η μεγαλύτερο</para>
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        internal static void CheckPagingParameters(ref int pageIndex, ref int pageSize)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentException(SR.GetString(SR.PageIndex_must_be_greatter_than_zero, "pageIndex"), "pageIndex");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException(SR.GetString(SR.PageSize_must_be_atleast_one, "pageSize"), "pageSize");
            }
        }


        #region Date and Time Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        internal static DateTime RoundToSeconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        internal static DateTime UtcNow()
        {
            //return RoundToSeconds(DateTime.UtcNow);

            return (DateTime.UtcNow);
        }

        /// <summary>
        /// Unix time, or POSIX time, is a system for describing instants in time, 
        /// defined as the number of seconds elapsed since midnight Coordinated 
        /// Universal Time (UTC) of Thursday, January 1, 1970 
        /// </summary>
        static DateTime UnixEpoch = new DateTime(1970, 1, 1);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctime"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToDatetime(Int64 ctime)
        {
            TimeSpan span = TimeSpan.FromTicks(ctime * TimeSpan.TicksPerSecond);
            DateTime t = UnixEpoch.Add(span);
            return t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Int64 DatetimeToUnixTime(DateTime dt)
        {
            TimeSpan span = dt - (UnixEpoch);
            return Convert.ToInt64(span.TotalSeconds);
        }
        #endregion

        internal static string GenerateSenderVerificationCode(int length, System.Random random = null)
        {
            return GenerateRecipientKey(length, random);
        }
        internal static string GenerateWebLink(int length, System.Random random = null)
        {
            return GenerateRecipientKey(length, random);
        }
        internal static string GenerateRecipientKey(int length, System.Random random = null)
        {
            if (random == null)
            {
                random = new System.Random();
            }
            char[] key = new char[length];

            for (int i = 0; i < length; i++)
            {
                var _c = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                key[i] = _c;
            }

            return new string(key).ToUpperInvariant();
        }

        /// <summary>
        /// Επιστρέφει το absolutepath του συγκεκριμένου συλλέκτη/survey.
        /// <para>O συλλέκτης πρέπει να είναι τύπου WebLink</para>
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="collector"></param>
        /// <param name="manualEntry"></param>
        /// <returns></returns>
        internal static string GetSurveyRuntimeAbsolutePath(VLSurvey survey, VLCollector collector, bool manualEntry)
        {
            if (collector.CollectorType != CollectorType.WebLink)
            {
                throw new VLException(string.Format("Collector '{0}', has wrong type!", collector.Name));
            }

            if (manualEntry == false)
            {
                var absolutePath = string.Format(@"/w/{0}/{1}/", collector.WebLink, BuiltinLanguages.GetTwoLetterISOCode(collector.TextsLanguage));
                return absolutePath;
            }
            else
            {
                var absolutePath = string.Format(@"/wm/{0}/{1}/", collector.WebLink, BuiltinLanguages.GetTwoLetterISOCode(collector.TextsLanguage));
                return absolutePath;
            }
        }
        /// <summary>
        /// Επιστρέφει το url του συγκεκριμένου συλλέκτη/survey.
        /// <para>O συλλέκτης πρέπει να είναι τύπου WebLink</para>
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="collector"></param>
        /// <param name="manualEntry"></param>
        /// <returns></returns>
        internal static string GetSurveyRuntimeURL(VLSurvey survey, VLCollector collector, bool manualEntry = false)
        {
            var host = ValisSystem.Settings.Core.RuntimeEngine.Host;
            var absolutePath = GetSurveyRuntimeAbsolutePath(survey, collector, manualEntry);

            var url = string.Format(@"{0}{1}", host, absolutePath);
            if (collector.UseSSL)
            {
                return string.Format("https://{0}", url);
            }
            else
            {
                return string.Format("http://{0}", url);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="collector"></param>
        /// <param name="manualEntry"></param>
        /// <returns></returns>
        internal static string GetSurveyRuntimeLink(VLSurvey survey, VLCollector collector, bool manualEntry = false)
        {
            var url = GetSurveyRuntimeURL(survey, collector, manualEntry);

            return string.Format("<a href=\"{0}\">{1}</a>", url, WebUtility.HtmlEncode(url));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="collector"></param>
        /// <param name="recipient"></param>
        /// <param name="manualEntry"></param>
        /// <returns></returns>
        internal static string GetSurveyRuntimeAbsolutePath(VLSurvey survey, VLCollector collector, VLRecipient recipient, bool manualEntry)
        {
            if (collector.CollectorType != CollectorType.Email)
            {
                throw new VLException(string.Format("Collector '{0}', has wrong type!", collector.Name));
            }

            if (manualEntry == false)
            {
                var absolutePath = string.Format(@"/em/{0}/{1}/{2}/{3}/", survey.PublicId, recipient.RecipientKey, collector.CollectorId, BuiltinLanguages.GetTwoLetterISOCode(collector.TextsLanguage));
                return absolutePath;
            }
            else
            {
                var absolutePath = string.Format(@"/emm/{0}/{1}/{2}/{3}/", survey.PublicId, recipient.RecipientKey, collector.CollectorId, BuiltinLanguages.GetTwoLetterISOCode(collector.TextsLanguage));
                return absolutePath;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="collector"></param>
        /// <param name="recipient"></param>
        /// <param name="manualEntry"></param>
        /// <returns></returns>
        internal static string GetSurveyRuntimeURL(VLSurvey survey, VLCollector collector, VLRecipient recipient, bool manualEntry = false)
        {
            var host = ValisSystem.Settings.Core.RuntimeEngine.Host;
            var absolutePath = GetSurveyRuntimeAbsolutePath(survey, collector, recipient, manualEntry);

            var url = string.Format(@"{0}{1}", host, absolutePath);
            if (collector.UseSSL)
            {
                return string.Format("https://{0}", url);
            }
            else
            {
                return string.Format("http://{0}", url);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="collector"></param>
        /// <param name="recipient"></param>
        /// <param name="manualEntry"></param>
        /// <returns></returns>
        internal static string GetSurveyRuntimeLink(VLSurvey survey, VLCollector collector, VLRecipient recipient, bool manualEntry = false)
        {
            var url = GetSurveyRuntimeURL(survey, collector, recipient, manualEntry);

            return string.Format("<a href=\"{0}\">{1}</a>", url, WebUtility.HtmlEncode(url));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        internal static string GetSurveyPreviewAbsolutePath(VLSurvey survey)
        {
            var absolutePath = string.Format(@"/s/{0}/{1}/", survey.PublicId, BuiltinLanguages.GetTwoLetterISOCode(survey.TextsLanguage));
            return absolutePath;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        internal static string GetSurveyPreviewURL(VLSurvey survey)
        {
            var host = ValisSystem.Settings.Core.RuntimeEngine.Host;
            var absolutePath = GetSurveyPreviewAbsolutePath(survey);

            var url = string.Format(@"{0}{1}", host, absolutePath);
            return string.Format("http://{0}", url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        internal static string GetSurveyPreviewLink(VLSurvey survey)
        {
            var url = GetSurveyPreviewURL(survey);

            return string.Format("<a href=\"{0}\">{1}</a>", url, WebUtility.HtmlEncode(url));
        }



        /// <summary>
        /// Επιστρέφει ένα URL για την αφαίρεση του συγκεκριμένου recipient απο την address list του συγκεκριμένου πελάτη που τρέχει το survey.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="collector"></param>
        /// <param name="message"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        internal static string GetRemoveRecipientURL(VLSurvey survey, VLCollector collector, VLMessage message, VLRecipient recipient)
        {
            var host = ValisSystem.Settings.Core.SystemPublicHostName;
            var _url  = ValisSystem.Settings.Core.RemoveUrl.Url;

            var url = string.Format(@"{0}?rkey={1}&pid={2}&cid={3}&lang={4}", _url, recipient.RecipientKey, survey.PublicId, collector.CollectorId, BuiltinLanguages.PrimaryLanguage.LanguageId);
            url = string.Format("{0}/{1}", host, url).Replace("//", "/");
            return string.Format("http://{0}", url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="collector"></param>
        /// <param name="message"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        internal static string GetRemoveRecipientLink(VLSurvey survey, VLCollector collector, VLMessage message, VLRecipient recipient)
        {
            var url = GetRemoveRecipientURL(survey, collector, message, recipient);

            return string.Format("<a href=\"{0}\">{0}</a>", url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static string GetVerifySenderURL(VLMessage message)
        {
            var host = ValisSystem.Settings.Core.SystemPublicHostName;
            var _url = ValisSystem.Settings.Core.VerifyUrl.Url;

            var url = string.Format(@"{0}?code={1}", _url, message.SenderVerificationCode);
            url = string.Format("{0}/{1}", host, url).Replace("//", "/");
            return string.Format("http://{0}", url);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static string GenerateSalt()
        {
            byte[] buf = new byte[16];
            using (var s = new RNGCryptoServiceProvider())
            {
                s.GetBytes(buf);
            }
            return Convert.ToBase64String(buf);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="pswdFormat"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        internal static string EncodePassword(string pass, VLPasswordFormat pswdFormat, string salt)
        {
            if (pswdFormat == VLPasswordFormat.Clear)
                return pass;

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet = null;

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);

            if (pswdFormat == VLPasswordFormat.Hashed)
            {
                using (HashAlgorithm s = HashAlgorithm.Create())
                {
                    bRet = s.ComputeHash(bAll);
                }
            }
            else
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown value for PasswordFormat ('{0}')!", pswdFormat.ToString()), "pswdFormat");
            }

            return Convert.ToBase64String(bRet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string ConvertToLower(string value)
        {
            if (value == null)
                return null;
            return value.ToLowerInvariant();
        }



        /// <summary>
        /// Serializes the objectGraph.
        /// <para>It uses the BinaryFormatter and the GZipStream for the compression.</para>
        /// </summary>
        /// <param name="objectGraph"></param>
        /// <param name="compressStream"></param>
        /// <returns></returns>
        internal static byte[] SerializeObject(object objectGraph, bool compressStream)
        {
            if (objectGraph == null) throw new ArgumentNullException("objectGraph");

            if (compressStream == true)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Compress))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(zipStream, objectGraph);
                        zipStream.Close();

                        return stream.ToArray();
                    }
                }
            }
            else
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, objectGraph);

                    return stream.ToArray();
                }
            }
        }
        /// <summary>
        /// Deserialize the objectBytes.
        /// <para>It uses the BinaryFormatter and the GZipStream for the decompression.</para>
        /// </summary>
        /// <param name="objectBytes"></param>
        /// <param name="decompressStream"></param>
        /// <returns></returns>
        internal static Object DeserializeObject(byte[] objectBytes, bool decompressStream)
        {
            if (objectBytes == null) throw new ArgumentNullException("objectBytes");


            if (decompressStream == true)
            {
                using (MemoryStream stream = new MemoryStream(objectBytes))
                {
                    using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return formatter.Deserialize(zipStream);
                    }
                }
            }
            else
            {
                using (MemoryStream stream = new MemoryStream(objectBytes))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream);
                }
            }
        }



        internal static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        public static string TruncateString(string input, Int32 maxLength = 67)
        {
            if (input.Length <= 67)
                return input;

            return input.Substring(0, 64-3) + "...";
        }

        /// <summary>
        /// This will check if user is in the local Administrators group 
        /// </summary>
        /// <returns></returns>
        public static bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin;
            try
            {
                //get the currently logged in user
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string UnWindExceptionContent(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            var _exception = exception;
            StringBuilder sb = new StringBuilder();
            bool addSeparator = false;
            while (_exception != null)
            {
                if (addSeparator)
                {
                    sb.Append(" --> ");
                }
                sb.AppendFormat("{0}:{1}", _exception.GetType().Name, _exception.Message);
                addSeparator = true;

                _exception = _exception.InnerException;
            }
            if (!string.IsNullOrWhiteSpace(exception.StackTrace))
            {
                sb.AppendFormat(" StackTrace: {0}", exception.StackTrace);
            }

            return sb.ToString();
        }
    }
}
