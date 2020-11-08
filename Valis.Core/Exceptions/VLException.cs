using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace Valis.Core
{
    /// <summary>
    /// Represents an exception in Valis.Core.
    /// <para>The VLException class is generally used to indicate a violation of a requirement in Valis Core.</para>
    /// </summary>
    [Serializable]
    public class VLException : Exception
    {
        private static System.Random m_random = new Random();
        private static int NextRefId()
        {
            lock (m_random) return m_random.Next(10000000, 1000000000);
        }

        Int32 m_referenceId = NextRefId();
        /// <summary>
        /// 
        /// </summary>
        public Int32 ReferenceId
        {
            get
            {
                return m_referenceId;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Valis.Core.VLException class.
        /// <para>This member is reserved for internal use and is not intended to be used directly by user code.</para>
        /// </summary>
        public VLException()
            : base("Error in the application.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the  Valis.Core.VLException class with a specified error message.
        /// <para>This member is reserved for internal use and is not intended to be used directly by user code.</para>
        /// </summary>
        /// <param name="message"> A message that describes the error.</param>
        public VLException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="referenceId"></param>
        public VLException(string message, Int32 referenceId)
            : base(message)
        {
            this.m_referenceId = referenceId;
        }

        /// <summary>
        /// Initializes a new instance of the Valis.Core.VLException class with serialized data.
        /// <para>This protected constructor is used for deserialization and is not intended to be used directly by user code.</para>
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context"> The contextual information about the source or destination.</param>
        [SecuritySafeCritical]
        protected VLException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Valis.Core.VLException class with
        /// a specified error message and a reference to the inner exception that is
        /// the cause of this exception.
        /// </summary>
        /// <param name="message"> The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException
        /// parameter is not a null reference, the current exception is raised in a catch
        /// block that handles the inner exception.</param>
        public VLException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// GetObjectData performs a custom serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
