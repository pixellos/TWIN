using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebLedMatrix.Controllers
{
    public class RoleResultException : Exception
    {
        public RoleResultException(){}

        public RoleResultException(string message) : base(message){}

        public RoleResultException(string message, Exception innerException) : base(message, innerException){}

        protected RoleResultException(SerializationInfo info, StreamingContext context) : base(info, context){}

        public IEnumerable<string> ErrorStrings { get; set; }
    }
}