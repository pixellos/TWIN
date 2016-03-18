using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;
using WebLedMatrix.Logic.Authentication.Models;

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