using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Valis.Core
{
    /// <summary>
    /// Represents an exception that occurs when a user has 
    /// no access rights upon a securable object.
    /// </summary>
    [Serializable]
    public sealed class VLAccessDeniedException : VLSecurityException, ISerializable
    {
        VLPermissions m_requiredPermissions;
        VLPermissions[] m_requiredPermissionsArray;
        bool m_OkIfOneSetExists;


        /// <summary>
        /// 
        /// </summary>
        public VLAccessDeniedException()
            : base("Access denied.")
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public VLAccessDeniedException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public VLAccessDeniedException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requiredPermissions"></param>
        public VLAccessDeniedException(VLPermissions requiredPermissions)
            : base("Access denied.")
        {
            m_requiredPermissions = requiredPermissions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requiredPermissions"></param>
        /// <param name="message"></param>
        public VLAccessDeniedException(VLPermissions requiredPermissions, string message)
            : base(message)
        {
            m_requiredPermissions = requiredPermissions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requiredPermissions"></param>
        public VLAccessDeniedException(VLPermissions[] requiredPermissions)
            : base("Access denied.")
        {
            m_requiredPermissionsArray = requiredPermissions;
            m_OkIfOneSetExists = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requiredPermissions"></param>
        /// <param name="message"></param>
        public VLAccessDeniedException(VLPermissions[] requiredPermissions, string message)
            : base(message)
        {
            m_requiredPermissionsArray = requiredPermissions;
            m_OkIfOneSetExists = true;
        }

        /// <summary>
        /// This constructor is used for deserialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private VLAccessDeniedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.m_OkIfOneSetExists = info.GetBoolean("OkIfOneSetExists");
            this.m_requiredPermissions = (VLPermissions)info.GetValue("requiredPermissions", typeof(VLPermissions));
            this.m_requiredPermissionsArray = (VLPermissions[])info.GetValue("requiredPermissionsArray", typeof(VLPermissions[]));
        }

        /// <summary>
        /// GetObjectData performs a custom serialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            base.GetObjectData(info, context);

            info.AddValue("OkIfOneSetExists", this.m_OkIfOneSetExists);
            info.AddValue("requiredPermissions", this.m_requiredPermissions, typeof(VLPermissions));
            info.AddValue("requiredPermissionsArray", this.m_requiredPermissionsArray, typeof(VLPermissions[]));
        }





        /// <summary>
        /// 
        /// </summary>
        public VLPermissions RequiredPermissions
        {
            get { return m_requiredPermissions; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Message
        {
            get
            {
                string message = base.Message;

                if (m_requiredPermissions != VLPermissions.None)
                {
                    string _requiredPerms = string.Format(CultureInfo.InvariantCulture, "Required Permissions: {0}", this.m_requiredPermissions.ToString());
                    return (message + Environment.NewLine + _requiredPerms);
                }
                else if (m_requiredPermissionsArray != null)
                {
                    StringBuilder sb = new StringBuilder();

                    if (m_requiredPermissionsArray.Length == 1)
                    {
                        sb.AppendFormat("Required Permissions: {0}", m_requiredPermissionsArray[0].ToString());
                    }
                    else
                    {
                        sb.AppendFormat("Required Permissions:{0}", Environment.NewLine);
                        for (int index = 0; index < m_requiredPermissionsArray.Length; index++)
                        {
                            if (index > 0)
                            {
                                sb.AppendFormat("{0}OR {0}", Environment.NewLine);
                            }
                            sb.AppendFormat(" ({0})", m_requiredPermissionsArray[index].ToString());
                        }
                    }

                    return (message + Environment.NewLine + sb.ToString());
                }

                return message;
            }
        }


    }
}
