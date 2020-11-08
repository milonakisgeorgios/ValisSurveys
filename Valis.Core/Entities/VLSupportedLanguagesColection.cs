using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Valis.Core
{
    public sealed class VLSupportedLanguagesColection : IEnumerable<VLLanguage>
    {
        internal Collection<VLLanguage> m_SupportedLanguages;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="supportedLanguagesIds"></param>
        public VLSupportedLanguagesColection(string supportedLanguagesIds)
        {
            m_SupportedLanguages = new Collection<VLLanguage>();
            if (!string.IsNullOrWhiteSpace(supportedLanguagesIds))
            {
                var tokens = supportedLanguagesIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string token in tokens)
                {
                    var language = BuiltinLanguages.GetLanguageById(Convert.ToInt16(token, CultureInfo.InvariantCulture), false);
                    if (language != null)
                    {
                        m_SupportedLanguages.Add(language);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        internal void Add(VLLanguage language)
        {
            if (language == null) throw new ArgumentNullException("language");
            m_SupportedLanguages.Add(language);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return m_SupportedLanguages.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<VLLanguage> GetEnumerator()
        {
            return m_SupportedLanguages.GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}
