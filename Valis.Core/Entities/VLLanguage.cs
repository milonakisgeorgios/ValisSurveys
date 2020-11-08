using System;
using System.Data.Common;
using System.Runtime.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// Αντιπροσωπεύει μία γλώσσα στο σύστημά μας
    /// <para>Οι γλώσσες που χρησιμοποιούμε είναι ουδέτερες languages.</para>
    /// <para>Υποστηρίζουμε ουδέτερες γλώσσες (π.χ. 'English') και όχι ζεύγη 
    /// 'English - United Kingdom' ή 'English - United States' δηλαδή γλώσσα και χώρα!</para>
    /// <para>To .NET framework μας δίνει CultureInfos και για τις ουδέτερες γλώσσες.</para>
    /// </summary>
    [Serializable]
    [DataContract]
    [CLSCompliant(true)]
    public sealed class VLLanguage
    {
        #region class fields
        Int16 m_languageId;
        String m_englishName;
        Int32 m_lCID;
        String m_name;
        String m_twoLetterISOCode;
        String m_threeLetterISOCode;
        #endregion



        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int16 LanguageId
        {
            get { return m_languageId; }
            internal set { m_languageId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String EnglishName
        {
            get { return m_englishName; }
            internal set { m_englishName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 LCID
        {
            get { return m_lCID; }
            internal set { m_lCID = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get { return m_name; }
            internal set { m_name = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String TwoLetterISOCode
        {
            get { return m_twoLetterISOCode; }
            internal set { m_twoLetterISOCode = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String ThreeLetterISOCode
        {
            get { return m_threeLetterISOCode; }
            internal set { m_threeLetterISOCode = value; }
        }
        #endregion
        


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        public VLLanguage()
        {
            m_languageId = default(Int16);
            m_englishName = default(string);
            m_lCID = default(Int32);
            m_name = default(string);
            m_twoLetterISOCode = default(string);
            m_threeLetterISOCode = default(string);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLLanguage(DbDataReader reader)
        {
            int ordinal = reader.GetOrdinal("LanguageId");
            this.LanguageId = reader.GetInt16(ordinal);
            ordinal = reader.GetOrdinal("EnglishName");
            this.EnglishName = reader.GetString(ordinal);
            ordinal = reader.GetOrdinal("LCID");
            this.LCID = reader.GetInt32(ordinal);
            ordinal = reader.GetOrdinal("Name");
            this.Name = reader.GetString(ordinal);
            ordinal = reader.GetOrdinal("TwoLetterISOCode");
            if (!reader.IsDBNull(ordinal)) this.TwoLetterISOCode = reader.GetString(ordinal);
            ordinal = reader.GetOrdinal("ThreeLetterISOCode");
            if (!reader.IsDBNull(ordinal)) this.ThreeLetterISOCode = reader.GetString(ordinal);
        }
        #endregion

        #region GetHashCode & Equals overrides
        /// <summary>
        /// Serves as a hash function for a particular type. GetHashCode is suitable for use in hashing algorithms and data structures like a hash table
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.LanguageId.GetHashCode() ^
                this.EnglishName.GetHashCode() ^
                this.LCID.GetHashCode() ^
                this.Name.GetHashCode() ^
                ((this.TwoLetterISOCode == null) ? string.Empty : this.TwoLetterISOCode.ToString()).GetHashCode() ^
                ((this.ThreeLetterISOCode == null) ? string.Empty : this.ThreeLetterISOCode.ToString()).GetHashCode();
        }
        /// <summary>
        /// Determines whether the specified Object is equal to the current Object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLLanguage)obj;

            //reference types
            if (!Object.Equals(EnglishName, other.EnglishName)) return false;
            if (!Object.Equals(Name, other.Name)) return false;
            if (!Object.Equals(TwoLetterISOCode, other.TwoLetterISOCode)) return false;
            if (!Object.Equals(ThreeLetterISOCode, other.ThreeLetterISOCode)) return false;
            //value types
            if (!LanguageId.Equals(other.LanguageId)) return false;
            if (!LCID.Equals(other.LCID)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLLanguage o1, VLLanguage o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLLanguage o1, VLLanguage o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_englishName, true, true, false, 512, "EnglishName");
            Utility.CheckParameter(ref m_name, true, true, false, 50, "Name");
            Utility.CheckParameter(ref m_twoLetterISOCode, false, false, false, 3, "TwoLetterISOCode");
            Utility.CheckParameter(ref m_threeLetterISOCode, false, false, false, 3, "ThreeLetterISOCode");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Implicitly returns an Int16 (LanguageId) from a LrLanguage.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static implicit operator Int16(VLLanguage language)
        {
            return language.LanguageId;
        }
    }
}
