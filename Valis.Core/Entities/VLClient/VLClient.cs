using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract, DataObject]
    [Serializable]
    public class VLClient : VLObject
    {

        [Flags]
        internal enum ClientAttributes : int
        {
            None            = 0,
            IsBuiltIn       = 1,        // 1 << 0
            AttributeXxx    = 2,        // 1 << 1
            AttributeYyy    = 4,        // 1 << 2
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_clientId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_code;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_profession;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_country;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_timeZoneId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_prefecture;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_town;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_address;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_zip;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_telephone1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_telephone2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_webSite;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_profile;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_comment;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_folderSequence;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_useCredits;
        #endregion


        #region EntityState
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        EntityState _currentEntityState = EntityState.Added;

        /// <summary>
        ///	Indicates state of object
        /// </summary>
        /// <remarks>0=Unchanged, 1=Added, 2=Changed</remarks>
        [BrowsableAttribute(false), XmlIgnoreAttribute()]
        public override EntityState EntityState
        {
            get { return _currentEntityState; }
            internal set { _currentEntityState = value; }
        }
        #endregion
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _deserializing = false;


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 ClientId
        {
            get { return this.m_clientId; }
            internal set
            {
                if (this.m_clientId == value)
                    return;

                this.m_clientId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Code
        {
            get { return this.m_code; }
            set
            {
                if (this.m_code == value)
                    return;

                this.m_code = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Name
        {
            get { return this.m_name; }
            set
            {
                if (this.m_name == value)
                    return;

                this.m_name = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Profession
        {
            get { return this.m_profession; }
            set
            {
                if (this.m_profession == value)
                    return;

                this.m_profession = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Country
        {
            get { return this.m_country; }
            set
            {
                if (this.m_country == value)
                    return;

                this.m_country = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String TimeZoneId
        {
            get { return this.m_timeZoneId; }
            set
            {
                if (this.m_timeZoneId == value)
                    return;

                this.m_timeZoneId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Prefecture
        {
            get { return this.m_prefecture; }
            set
            {
                if (this.m_prefecture == value)
                    return;

                this.m_prefecture = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Town
        {
            get { return this.m_town; }
            set
            {
                if (this.m_town == value)
                    return;

                this.m_town = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Address
        {
            get { return this.m_address; }
            set
            {
                if (this.m_address == value)
                    return;

                this.m_address = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Zip
        {
            get { return this.m_zip; }
            set
            {
                if (this.m_zip == value)
                    return;

                this.m_zip = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Telephone1
        {
            get { return this.m_telephone1; }
            set
            {
                if (this.m_telephone1 == value)
                    return;

                this.m_telephone1 = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Telephone2
        {
            get { return this.m_telephone2; }
            set
            {
                if (this.m_telephone2 == value)
                    return;

                this.m_telephone2 = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String WebSite
        {
            get { return this.m_webSite; }
            set
            {
                if (this.m_webSite == value)
                    return;

                this.m_webSite = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal System.Int32 AttributeFlags
        {
            get { return this.m_attributeFlags; }
            set
            {
                if (this.m_attributeFlags == value)
                    return;

                this.m_attributeFlags = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Το Profile του πελάτη. 
        /// <para>Το Profile θέτει όρια (quotas) στην χρήση του συστήματος και μας λέει εάν ο Πελάτης δαπανά ή όχι Credits για να χρησιμοποιήσει
        /// τις υπηρεσίες του συστήματος.</para>
        /// </summary>
        public Int32 Profile
        {
            get
            {
                return this.m_profile;
            }
            set
            {
                if (this.m_profile == value)
                    return;

                this.m_profile = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }


        public System.String Comment
        {
            get { return this.m_comment; }
            set
            {
                if (this.m_comment == value)
                    return;

                this.m_comment = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int16 FolderSequence
        {
            get { return this.m_folderSequence; }
        }


        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsBuiltIn
        {
            get { return (this.m_attributeFlags & ((int)ClientAttributes.IsBuiltIn)) == ((int)ClientAttributes.IsBuiltIn); }
            internal set
            {
                if (this.IsBuiltIn == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientAttributes.IsBuiltIn;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientAttributes.IsBuiltIn;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Οι πελάτες στο σύστημά μας χωρίζονται σε δύο κατηγορίες. Αυτούς που μπορούν να χρησιμοποιούν ελεύθερα τους πόρους του συστήματος,
        /// και σε αυτούς που αγοράζουν credits, για να τους χρησιμοποιήσουν. Αυτό ελέγχεται απο το Profile του εκάστοτε Πελάτη.
        /// <para>Aντιγραφή της τιμής απο το αντίστοιχο πεδίο του ClienProfile, την στιγμή που διαβάζεται ο client απο την βάση.</para>
        /// </summary>
        public System.Boolean UseCredits
        {
            get { return this.m_useCredits; }
        }
        #endregion

        #region class constructors
        internal VLClient()
        {
            m_clientId = default(Int32);
            m_code = default(string);
            m_name = default(string);
            m_profession = default(string);
            m_country = default(Int32);
            m_timeZoneId = default(string);
            m_prefecture = default(string);
            m_town = default(string);
            m_address = default(string);
            m_zip = default(string);
            m_telephone1 = default(string);
            m_telephone2 = default(string);
            m_webSite = default(string);
            m_attributeFlags = default(Int32);
            m_profile = BuiltinProfiles.Default.ProfileId;
            m_comment = default(string);
            m_folderSequence = default(Int16);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLClient(DbDataReader reader)
            : base(reader)
        {
            this.ClientId = reader.GetInt32(0);
            if (!reader.IsDBNull(1)) this.Code = reader.GetString(1);
            this.Name = reader.GetString(2);
            if (!reader.IsDBNull(3)) this.Profession = reader.GetString(3);
            this.Country = reader.GetInt32(4);
            if (!reader.IsDBNull(5)) this.TimeZoneId = reader.GetString(5);
            if (!reader.IsDBNull(6)) this.Prefecture = reader.GetString(6);
            if (!reader.IsDBNull(7)) this.Town = reader.GetString(7);
            if (!reader.IsDBNull(8)) this.Address = reader.GetString(8);
            if (!reader.IsDBNull(9)) this.Zip = reader.GetString(9);
            if (!reader.IsDBNull(10)) this.Telephone1 = reader.GetString(10);
            if (!reader.IsDBNull(11)) this.Telephone2 = reader.GetString(11);
            if (!reader.IsDBNull(12)) this.WebSite = reader.GetString(12);
            this.AttributeFlags = reader.GetInt32(13);
            this.Profile = reader.GetInt32(14);
            if (!reader.IsDBNull(15)) this.Comment = reader.GetString(15);
            this.m_folderSequence = reader.GetInt16(16);
            this.m_useCredits = Convert.ToBoolean(reader.GetInt32(17));

            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.ClientId.GetHashCode() ^
                ((this.Code == null) ? string.Empty : this.Code.ToString()).GetHashCode() ^
                this.Name.GetHashCode() ^
                ((this.Profession == null) ? string.Empty : this.Profession.ToString()).GetHashCode() ^
                this.Country.GetHashCode() ^
                ((this.TimeZoneId == null) ? string.Empty : this.TimeZoneId.ToString()).GetHashCode() ^
                ((this.Prefecture == null) ? string.Empty : this.Prefecture.ToString()).GetHashCode() ^
                ((this.Town == null) ? string.Empty : this.Town.ToString()).GetHashCode() ^
                ((this.Address == null) ? string.Empty : this.Address.ToString()).GetHashCode() ^
                ((this.Zip == null) ? string.Empty : this.Zip.ToString()).GetHashCode() ^
                ((this.Telephone1 == null) ? string.Empty : this.Telephone1.ToString()).GetHashCode() ^
                ((this.Telephone2 == null) ? string.Empty : this.Telephone2.ToString()).GetHashCode() ^
                ((this.WebSite == null) ? string.Empty : this.WebSite.ToString()).GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.Profile.GetHashCode() ^
                ((this.Comment == null) ? string.Empty : this.Comment.ToString()).GetHashCode() ^
                this.FolderSequence.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLClient)obj;

            //reference types
            if (!Object.Equals(m_code, other.m_code)) return false;
            if (!Object.Equals(m_name, other.m_name)) return false;
            if (!Object.Equals(m_profession, other.m_profession)) return false;
            if (!Object.Equals(m_timeZoneId, other.m_timeZoneId)) return false;
            if (!Object.Equals(m_prefecture, other.m_prefecture)) return false;
            if (!Object.Equals(m_town, other.m_town)) return false;
            if (!Object.Equals(m_address, other.m_address)) return false;
            if (!Object.Equals(m_zip, other.m_zip)) return false;
            if (!Object.Equals(m_telephone1, other.m_telephone1)) return false;
            if (!Object.Equals(m_telephone2, other.m_telephone2)) return false;
            if (!Object.Equals(m_webSite, other.m_webSite)) return false;
            if (!Object.Equals(m_comment, other.m_comment)) return false;
            //value types
            if (!m_clientId.Equals(other.m_clientId)) return false;
            if (!m_country.Equals(other.m_country)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;
            if (!m_profile.Equals(other.m_profile)) return false;
            if (!m_folderSequence.Equals(other.m_folderSequence)) return false;

            return true;
        }
        public static Boolean operator ==(VLClient o1, VLClient o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLClient o1, VLClient o2)
        {
            return !(o1 == o2);
        }

        #endregion

        
        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
			Utility.CheckParameter(ref m_code, false, false, false,128, "Code");
            ValidateName(ref m_name);
            Utility.CheckParameter(ref m_profession, false, false, false, 256, "Profession");
            Utility.CheckParameter(ref m_timeZoneId, true, true, true, 128, "TimeZoneId");
			Utility.CheckParameter(ref m_prefecture, false, false, false,128, "Prefecture");
			Utility.CheckParameter(ref m_town, false, false, false,128, "Town");
			Utility.CheckParameter(ref m_address, false, false, false,512, "Address");
			Utility.CheckParameter(ref m_zip, false, false, false,24, "Zip");
			Utility.CheckParameter(ref m_telephone1, false, false, false,128, "Telephone1");
			Utility.CheckParameter(ref m_telephone2, false, false, false,128, "Telephone2");
			Utility.CheckParameter(ref m_webSite, false, false, false,256, "WebSite");
			Utility.CheckParameter(ref m_comment, false, false, false,-1, "Comment");
        }
        internal static void ValidateName(ref string name)
        {
            Utility.CheckParameter(ref name, true, true, false, 512, "Name");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
