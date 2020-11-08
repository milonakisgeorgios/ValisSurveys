using System.Collections.ObjectModel;
using System.Globalization;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class BuiltinLanguages
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly VLLanguage AllLanguages = new VLLanguage { LanguageId = -4, EnglishName = "All Languages", LCID = -4, Name = string.Empty, TwoLetterISOCode = string.Empty, ThreeLetterISOCode = string.Empty };
        /// <summary>
        /// 
        /// </summary>
        public static readonly VLLanguage UnknownLanguage = new VLLanguage { LanguageId = -3, EnglishName = "Unknown Language", LCID = -3, Name = string.Empty, TwoLetterISOCode = string.Empty, ThreeLetterISOCode = string.Empty };
        /// <summary>
        /// αναπαριστά την default language του χρήστη της τρέχουσας συνεδρίας
        /// </summary>
        public static readonly VLLanguage DefaultLanguage = new VLLanguage { LanguageId = -2, EnglishName = "Default Language", LCID = -2, Name = string.Empty, TwoLetterISOCode = string.Empty, ThreeLetterISOCode = string.Empty };
        /// <summary>
        /// Αναπαριστά την γλωσσα με την οποία πρωτοδημιουργήθηκε κάθε survey.
        /// <para>Δεν έχει συγκεκριμένη τιμή, και κάθε survey μπορεί να έχει διαφορετική PrimaryLanguage απο κάποιο άλλο.</para>
        /// </summary>
        public static readonly VLLanguage PrimaryLanguage = new VLLanguage { LanguageId = -1, EnglishName = "Primary Language", LCID = -1, Name = string.Empty, TwoLetterISOCode = string.Empty, ThreeLetterISOCode = string.Empty };
        /// <summary>
        /// Αναπαριστά την τρέχουσα γλώσσα της συνεδρίας του τρέχοντος χρήστη
        /// </summary>
        public static readonly VLLanguage Invariant = new VLLanguage { LanguageId = 0, EnglishName = "Invariant Language", LCID = 127, Name = "inv", TwoLetterISOCode = "iv", ThreeLetterISOCode = "ivl" };

        public static readonly VLLanguage Bulgarian = new VLLanguage { LanguageId = 19, EnglishName = "Bulgarian", LCID = 2, Name = "bg", TwoLetterISOCode = "bg", ThreeLetterISOCode = "bul" };
        public static readonly VLLanguage English = new VLLanguage { LanguageId = 33, EnglishName = "English", LCID = 9, Name = "en", TwoLetterISOCode = "en", ThreeLetterISOCode = "eng" };
        public static readonly VLLanguage French = new VLLanguage { LanguageId = 38, EnglishName = "French", LCID = 12, Name = "fr", TwoLetterISOCode = "fr", ThreeLetterISOCode = "fra" };
        public static readonly VLLanguage German = new VLLanguage { LanguageId = 42, EnglishName = "German", LCID = 7, Name = "de", TwoLetterISOCode = "de", ThreeLetterISOCode = "deu" };
        public static readonly VLLanguage Greek = new VLLanguage { LanguageId = 43, EnglishName = "Greek", LCID = 8, Name = "el", TwoLetterISOCode = "el", ThreeLetterISOCode = "ell" };
        public static readonly VLLanguage Russian = new VLLanguage { LanguageId = 101, EnglishName = "Russian", LCID = 25, Name = "ru", TwoLetterISOCode = "ru", ThreeLetterISOCode = "rus" };

        internal static Collection<VLLanguage> s_languages;

        /// <summary>
        /// Gets a collection with all the languages this system currently supports
        /// </summary>
        public static Collection<VLLanguage> Languages { get { return s_languages; } }

        static BuiltinLanguages()
        {
            s_languages = new Collection<VLLanguage>();
            s_languages.Add(Invariant);
            s_languages.Add(Bulgarian);
            s_languages.Add(English);
            s_languages.Add(French);
            s_languages.Add(German);
            s_languages.Add(Greek);
            s_languages.Add(Russian);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="throwExceptionIfNotExists"></param>
        /// <returns></returns>
        public static VLLanguage GetLanguageById(short languageId, bool throwExceptionIfNotExists = true)
        {
            foreach (var item in s_languages)
            {
                if (item.LanguageId == languageId)
                    return item;
            }
            if (throwExceptionIfNotExists)
            {
                throw new VLException(string.Format(CultureInfo.InvariantCulture, "BuiltinLanguages.GetLanguageById() does not recognise languageid {0}!", languageId));
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="throwExceptionIfNotExists"></param>
        /// <returns></returns>
        public static string GetTwoLetterISOCode(short languageId, bool throwExceptionIfNotExists = true)
        {
            foreach (var item in s_languages)
            {
                if (item.LanguageId == languageId)
                    return item.TwoLetterISOCode;
            }
            if (throwExceptionIfNotExists)
            {
                throw new VLException(string.Format(CultureInfo.InvariantCulture, "BuiltinLanguages.GetTwoLetterISOCode() does not recognise languageid {0}!", languageId));
            }

            return null;
        }

        public static string GetLanguageThumbnail(short languageId)
        {
            switch (languageId)
            {
                case 0: return "flag0000.gif";
                //case 2: return "flag041c.gif";//Albanian
                //case 12: return "flag042d.gif";//Basque
                //case 13: return "flag0423.gif";//Belarusian
                case 19: return "flag0402.gif";//Bulgarian
                //case 20: return "flag0403.gif";//Catalan
                //case 21: return "flag0804.gif";//Chinese
                //case 27: return "flag041a.gif";//Croatian
                //case 28: return "flag0405.gif";//Czech
                //case 29: return "flag0406.gif";//Danish
                //case 32: return "flag0413.gif";//Dutch
                case 33: return "flag0809.gif";//English
                //case 34: return "flag0425.gif";//Estonian
                //case 37: return "flag040b.gif";//Finnish
                case 38: return "flag040c.gif";//French
                //case 41: return "flag0437.gif";//Georgian
                case 42: return "flag0407.gif";//German
                case 43: return "flag0408.gif";//Greek
                //case 48: return "flag040d.gif";//Hebrew
                //case 50: return "flag040e.gif";//Hungarian
                //case 51: return "flag040f.gif";//Icelandic
                //case 53: return "flag0421.gif";//Indonesian
                //case 60: return "flag0410.gif";//Italian
                //case 61: return "flag0411.gif";//Japanese
                //case 69: return "flag0412.gif";//Korean
                //case 72: return "flag0426.gif";//Latvian
               // case 73: return "flag0427.gif";//Lithuanian
                //case 75: return "flag046e.gif";//Luxembourgish
                //case 77: return "flag043e.gif";//Malay
                //case 88: return "flag0414.gif";//Norwegian
                //case 95: return "flag0415.gif";//Polish
                //case 96: return "flag0816.gif";//Portuguese
                //case 99: return "flag0418.gif";//Romanian
                case 101: return "flag0419.gif";//Russian
                //case 115: return "flag041b.gif";//Slovak
                //case 117: return "flag200a.gif";//Spanish
                //case 118: return "flag041d.gif";//Swedish
                //case 129: return "flag041f.gif";//Turkish
            }
            return "flag9999.png";
        }
    }
}
