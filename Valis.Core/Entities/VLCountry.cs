using System;
using System.Data.Common;

namespace Valis.Core
{
    public sealed class VLCountry
    {
        #region class fields
        Int32 m_countryId;
        String m_name;
        #endregion


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32 CountryId
        {
            get { return m_countryId; }
            set { m_countryId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get { return m_name; }
            internal set { m_name = value; }
        }
        #endregion


        #region class constructors
        public VLCountry()
        {
            m_countryId = default(Int32);
            m_name = default(string);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLCountry(DbDataReader reader)
        {
            this.CountryId = reader.GetInt32(0);
            this.Name = reader.GetString(1);

        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.CountryId.GetHashCode() ^
                this.Name.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLCountry)obj;

            //reference types
            if (!Object.Equals(m_name, other.m_name)) return false;
            //value types
            if (!m_countryId.Equals(other.m_countryId)) return false;

            return true;
        }
        public static Boolean operator ==(VLCountry o1, VLCountry o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLCountry o1, VLCountry o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public static implicit operator Int32(VLCountry country)
        {
            return country.CountryId;
        }
    }
}
