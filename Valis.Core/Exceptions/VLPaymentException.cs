using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Valis.Core
{
    [Serializable]
    public class VLPaymentException : VLException
    {
        /// <summary>
        /// Initializes a new instance of the VLPaymentException class. This is the default constructor.
        /// </summary>
        public VLPaymentException()
            : base("Payment Exception")
        {
        }

        /// <summary>
        /// Initializes a new instance of the VLPaymentException class with the specified string.
        /// </summary>
        /// <param name="message">The string to display when the exception is thrown.</param>
        public VLPaymentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the VLPaymentException class with the specified serialization information and context.
        /// <para>This protected constructor is used for deserialization and is not intended to be used directly by user code.</para>
        /// </summary>
        /// <param name="info">The data necessary to serialize or deserialize an object.</param>
        /// <param name="context">Description of the source and destination of the specified serialized stream.</param>
        private VLPaymentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the VLPaymentException class with the specified string and inner exception.
        /// </summary>
        /// <param name="message">The string to display when the exception is thrown.</param>
        /// <param name="innerException">A reference to an inner exception.</param>
        public VLPaymentException(string message, Exception innerException)
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
