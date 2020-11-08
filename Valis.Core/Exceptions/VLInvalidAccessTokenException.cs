using System;
using System.Runtime.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// Represents the exception that is thrown when an invalid accessToken is detected by the security subsystem..
    /// </summary>
    public sealed class VLInvalidAccessTokenException : VLSecurityException
    {
        /// <summary>
        /// Initializes a new instance of the VLInvalidAccessTokenException class. This is the default constructor.
        /// </summary>
        public VLInvalidAccessTokenException()
            : base("Invalid accessToken")
        {
        }

        /// <summary>
        /// Initializes a new instance of the VLInvalidAccessTokenException class with the specified string.
        /// </summary>
        /// <param name="message">The string to display when the exception is thrown.</param>
        public VLInvalidAccessTokenException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the VLInvalidAccessTokenException class with the specified serialization information and context.
        /// <para>This protected constructor is used for deserialization and is not intended to be used directly by user code.</para>
        /// </summary>
        /// <param name="info">The data necessary to serialize or deserialize an object.</param>
        /// <param name="context">Description of the source and destination of the specified serialized stream.</param>
        private VLInvalidAccessTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the VLInvalidAccessTokenException class with the specified string and inner exception.
        /// </summary>
        /// <param name="message">The string to display when the exception is thrown.</param>
        /// <param name="innerException">A reference to an inner exception.</param>
        public VLInvalidAccessTokenException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}
