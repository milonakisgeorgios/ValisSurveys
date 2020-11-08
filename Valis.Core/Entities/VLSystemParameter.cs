using System;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract, DataObject]
    public sealed class VLSystemParameter : VLObject
    {
        [Flags]
        internal enum SystemParameterAttributes : int
        {
            None = 0,
            IsBuiltIn = 1,          // 1 << 0
            IsHidden = 2,          // 1 << 1
        }


        #region class fields
        [DataMember]
        Guid m_parameterId;
        [DataMember]
        String m_parameterKey;
        [DataMember]
        String m_parameterValue;
        [DataMember]
        VLParameterType m_parameterType;
        [DataMember]
        Int32 m_attributeFlags;
        #endregion


        #region EntityState
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
        bool _deserializing = false;


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public System.Guid ParameterId
        {
            get { return this.m_parameterId; }
            internal set
            {
                if (this.m_parameterId == value)
                    return;

                this.m_parameterId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String ParameterKey
        {
            get { return this.m_parameterKey; }
            set
            {
                if (this.m_parameterKey == value)
                    return;

                this.m_parameterKey = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String ParameterValue
        {
            get { return this.m_parameterValue; }
            set
            {
                if (this.m_parameterValue == value)
                    return;

                this.m_parameterValue = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public VLParameterType ParameterType
        {
            get { return this.m_parameterType; }
            set
            {
                if (this.m_parameterType == value)
                    return;

                this.m_parameterType = value;
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
        /// Gets a Boolean value that specifies whether the SystemParameter is a built-in item.
        /// <para>Built-in values are defined during the installation of the system, and cannot be altered by any means.</para>
        /// </summary>
        public System.Boolean IsBuiltIn
        {
            get { return (this.m_attributeFlags & ((int)SystemParameterAttributes.IsBuiltIn)) == ((int)SystemParameterAttributes.IsBuiltIn); }
            internal set
            {
                if (this.IsBuiltIn == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SystemParameterAttributes.IsBuiltIn;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SystemParameterAttributes.IsBuiltIn;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Gets or sets a Boolean value that specifies whether the SystemParameter is a hidden item.
        /// <para>Hidden values are not shown to the end-users</para>
        /// </summary>
        internal System.Boolean IsHidden
        {
            get { return (this.m_attributeFlags & ((int)SystemParameterAttributes.IsHidden)) == ((int)SystemParameterAttributes.IsHidden); }
            set
            {
                if (this.IsHidden == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SystemParameterAttributes.IsHidden;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SystemParameterAttributes.IsHidden;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLSystemParameter()
        {
            m_parameterId = Guid.NewGuid();
            m_parameterKey = default(string);
            m_parameterValue = default(string);
            m_parameterType = default(Byte);
            m_attributeFlags = default(Int32);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLSystemParameter(DbDataReader reader)
            : base(reader)
        {
            this.ParameterId = reader.GetGuid(0);
            this.ParameterKey = reader.GetString(1);
            this.ParameterValue = reader.GetString(2);
            this.ParameterType = (VLParameterType)reader.GetByte(3);
            this.AttributeFlags = reader.GetInt32(4);

        }
        #endregion


        #region GetHashCode & Equals overrides
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.ParameterId.GetHashCode() ^
                this.ParameterKey.GetHashCode() ^
                this.ParameterValue.GetHashCode() ^
                this.ParameterType.GetHashCode() ^
                this.AttributeFlags.GetHashCode();
        }
        /// <summary>
        /// 
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


            var other = (VLSystemParameter)obj;

            //reference types
            if (!Object.Equals(m_parameterKey, other.m_parameterKey)) return false;
            if (!Object.Equals(m_parameterValue, other.m_parameterValue)) return false;
            //value types
            if (!m_parameterId.Equals(other.m_parameterId)) return false;
            if (!m_parameterType.Equals(other.m_parameterType)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLSystemParameter o1, VLSystemParameter o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLSystemParameter o1, VLSystemParameter o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            ValidateParameterKey(ref m_parameterKey);
            Utility.CheckParameter(ref m_parameterValue, true, true, false, 2048, "ParameterValue");

            switch (this.ParameterType)
            {
                case VLParameterType.StringType:
                    break;
                case VLParameterType.Int32Type:
                    {
                        int _value = 0;
                        if (!int.TryParse(this.ParameterValue, System.Globalization.NumberStyles.Integer, CultureInfo.InvariantCulture, out _value))
                        {
                            throw new VLException(string.Format(CultureInfo.InvariantCulture, "Value '{0}' is not an integer!", this.ParameterValue));
                        }
                    }
                    break;
                case VLParameterType.NumberType:
                    {
                        double _value = 0;
                        if (!double.TryParse(this.ParameterValue, System.Globalization.NumberStyles.Number, CultureInfo.InvariantCulture, out _value))
                        {
                            throw new VLException(string.Format(CultureInfo.InvariantCulture, "Value '{0}' is not a double!", this.ParameterValue));
                        }
                    }
                    break;
                case VLParameterType.GuidType:
                    {
                        Guid _value = Guid.Empty;
                        if (!Guid.TryParse(this.ParameterValue, out _value))
                        {
                            throw new VLException(string.Format(CultureInfo.InvariantCulture, "Value '{0}' is not a Guid!", this.ParameterValue));
                        }
                    }
                    break;
                case VLParameterType.DateType:
                    {
                        DateTime _value = DateTime.MinValue;
                        if (!DateTime.TryParse(this.ParameterValue, CultureInfo.InvariantCulture, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowWhiteSpaces, out _value))
                        {
                            throw new VLException(string.Format(CultureInfo.InvariantCulture, "Value '{0}' is not a DateTime!", this.ParameterValue));
                        }
                    }
                    break;
                case VLParameterType.BooleanType:
                    {
                        bool _value = false;
                        if (!bool.TryParse(this.ParameterValue, out _value))
                        {
                            throw new VLException(string.Format(CultureInfo.InvariantCulture, "Value '{0}' is not a Boolean!", this.ParameterValue));
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterKey"></param>
        internal static void ValidateParameterKey(ref string parameterKey)
        {
            Utility.CheckParameter(ref parameterKey, true, true, false, 128, "ParameterKey");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", this.ParameterKey, this.ParameterValue);
        }
    }
}
