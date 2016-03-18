using System;
using System.Runtime.Serialization;

namespace WebLedMatrix.Controllers
{
    class InputDataException : Exception
    {
        public InputDataException()
        {
        }

        public InputDataException(string message) : base(message)
        {
        }

        public InputDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InputDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}