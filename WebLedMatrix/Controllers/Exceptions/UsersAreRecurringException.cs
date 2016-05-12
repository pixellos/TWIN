using System;
using System.Runtime.Serialization;

namespace WebLedMatrix.Controllers
{
    public class UsersAreRecurringException : Exception
    {
        public UsersAreRecurringException(){}

        public UsersAreRecurringException(string message) : base(message){}

        public UsersAreRecurringException(string message, Exception innerException) : base(message, innerException){}

        protected UsersAreRecurringException(SerializationInfo info, StreamingContext context) : base(info, context){}
    }
}